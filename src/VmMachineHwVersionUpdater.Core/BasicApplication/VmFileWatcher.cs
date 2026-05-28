using System.Collections.Concurrent;
using EvilBaschdi.Core.Extensions;

namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <inheritdoc cref="IVmFileWatcher" />
public class VmFileWatcher(
    [NotNull] IPathSettings pathSettings) : IVmFileWatcher
{
    private readonly IPathSettings
        _pathSettings = pathSettings ?? throw new ArgumentNullException(nameof(pathSettings));

    private readonly List<FileSystemWatcher> _watchers = [];

    private readonly ConcurrentDictionary<string, CancellationTokenSource> _debounceTokens =
        new(StringComparer.OrdinalIgnoreCase);

    private readonly TimeSpan _debounceInterval = TimeSpan.FromMilliseconds(1000);
    private bool _disposed;

    /// <inheritdoc />
    public event Action<VmFileChangedEventArgs> FileChanged;

    /// <inheritdoc />
    public void Start()
    {
        Stop();

        var machinePoolPaths = _pathSettings.VmPool;
        var archivePoolPaths = _pathSettings.ArchivePath;

        machinePoolPaths.AddRange(archivePoolPaths);

        var existingPaths = machinePoolPaths.GetExistingDirectories();

        foreach (var path in existingPaths)
        {
            CreateWatcher(path, "*.vmx");
            CreateWatcher(path, "*.vbox");
        }
    }

    /// <inheritdoc />
    public void Stop()
    {
        foreach (var watcher in _watchers)
        {
            watcher.EnableRaisingEvents = false;
            watcher.Dispose();
        }

        _watchers.Clear();

        foreach (var cts in _debounceTokens.Values)
        {
            cts.Cancel();
            cts.Dispose();
        }

        _debounceTokens.Clear();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        Stop();
        _disposed = true;
        GC.SuppressFinalize(this);
    }

    private void CreateWatcher(string path, string filter)
    {
        var watcher = new FileSystemWatcher(path, filter)
                      {
                          NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName,
                          IncludeSubdirectories = true,
                          EnableRaisingEvents = true
                      };

        watcher.Changed += OnChanged;
        watcher.Created += OnCreated;
        watcher.Deleted += OnDeleted;
        watcher.Renamed += OnRenamed;

        _watchers.Add(watcher);
    }

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        DebounceAndRaise(e.FullPath, VmFileChangeType.Changed);
    }

    private void OnCreated(object sender, FileSystemEventArgs e)
    {
        DebounceAndRaise(e.FullPath, VmFileChangeType.Created);
    }

    private void OnDeleted(object sender, FileSystemEventArgs e)
    {
        DebounceAndRaise(e.FullPath, VmFileChangeType.Deleted);
    }

    private void OnRenamed(object sender, RenamedEventArgs e)
    {
        DebounceAndRaise(e.FullPath, VmFileChangeType.Renamed, e.OldFullPath);
    }

    private void DebounceAndRaise(string filePath, VmFileChangeType changeType, string oldFilePath = null)
    {
        if (_disposed)
        {
            return;
        }

        var key = filePath;

        if (_debounceTokens.TryRemove(key, out var existingCts))
        {
            existingCts.Cancel();
        }

        var cts = new CancellationTokenSource();
        var token = cts.Token;
        _debounceTokens[key] = cts;

        _ = Task.Delay(_debounceInterval, token).ContinueWith(
            _ =>
            {
                if (_debounceTokens.TryRemove(key, out var removedCts))
                {
                    removedCts.Dispose();
                }

                FileChanged?.Invoke(new VmFileChangedEventArgs
                                    {
                                        FilePath = filePath,
                                        OldFilePath = oldFilePath,
                                        ChangeType = changeType
                                    });
            },
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnRanToCompletion,
            TaskScheduler.Default);
    }
}