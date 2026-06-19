using EvilBaschdi.Core.Extensions;
using Microsoft.Extensions.Logging;

namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <inheritdoc cref="IVmFileWatcher" />
public class VmFileWatcher(
    [NotNull] IPathSettings pathSettings,
    [NotNull] IFileChangeDebouncer fileChangeDebouncer,
    [NotNull] ILogger<VmFileWatcher> logger) : IVmFileWatcher
{
    private readonly IPathSettings
        _pathSettings = pathSettings ?? throw new ArgumentNullException(nameof(pathSettings));

    private readonly IFileChangeDebouncer _fileChangeDebouncer =
        fileChangeDebouncer ?? throw new ArgumentNullException(nameof(fileChangeDebouncer));

    private readonly ILogger<VmFileWatcher> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly List<FileSystemWatcher> _watchers = [];
    private readonly ReaderWriterLockSlim _disposeLock = new();
    private bool _disposed;

    /// <inheritdoc />
    public event Action<VmFileChangedEventArgs> FileChanged;

    /// <inheritdoc />
    public void Start()
    {
        _disposeLock.EnterReadLock();
        try
        {
            if (_disposed)
            {
                _logger.LogWarning("VmFileWatcher.Start() called after disposal");
                return;
            }

            Stop();

            var machinePoolPaths = _pathSettings.VmPool;
            var archivePoolPaths = _pathSettings.ArchivePath;

            machinePoolPaths.AddRange(archivePoolPaths);

            var existingPaths = machinePoolPaths.GetExistingDirectories();
            _logger.LogInformation("Starting VmFileWatcher for {PathCount} directories", existingPaths.Count);

            foreach (var path in existingPaths)
            {
                CreateWatcher(path, "*.vmx");
                CreateWatcher(path, "*.vbox");
                CreateWatcher(path, "*.log");
                CreateWatcher(path, "*.vmss");
            }

            _logger.LogInformation("VmFileWatcher started successfully");
        }
        finally
        {
            _disposeLock.ExitReadLock();
        }
    }

    /// <inheritdoc />
    public void Stop()
    {
        _logger.LogInformation("Stopping VmFileWatcher");

        foreach (var watcher in _watchers)
        {
            try
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error disposing FileSystemWatcher");
            }
        }

        _watchers.Clear();
        _fileChangeDebouncer.CancelAll();
        _logger.LogInformation("VmFileWatcher stopped");
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        try
        {
            _disposeLock.EnterWriteLock();
            try
            {
                if (_disposed)
                {
                    return;
                }

                _logger.LogDebug("Disposing VmFileWatcher");
                Stop();
                _disposed = true;
                GC.SuppressFinalize(this);
            }
            finally
            {
                _disposeLock.ExitWriteLock();
            }
        }
        finally
        {
            try
            {
                _disposeLock?.Dispose();
            }
            catch
            {
                // Ignore disposal errors
            }
        }
    }

    private void CreateWatcher(string path, string filter)
    {
        try
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
            _logger.LogDebug("Created FileSystemWatcher for {Path} with filter {Filter}", path, filter);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to create FileSystemWatcher for {Path} with filter {Filter}. The path may be inaccessible.",
                path, filter);
        }
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
        _disposeLock.EnterReadLock();
        try
        {
            if (_disposed)
            {
                return;
            }

            if (!IsValidFileExtension(filePath))
            {
                _logger.LogDebug("Ignoring file change for {FilePath} - extension not in allowed list", filePath);
                return;
            }

            _logger.LogDebug("Debouncing file change: {FilePath} ({ChangeType})", filePath, changeType);

            _fileChangeDebouncer.Debounce(filePath, () =>
                                                    {
                                                        try
                                                        {
                                                            _logger.LogDebug("Raising FileChanged event for {FilePath} ({ChangeType})", filePath, changeType);
                                                            FileChanged?.Invoke(new VmFileChangedEventArgs
                                                                                {
                                                                                    FilePath = filePath,
                                                                                    OldFilePath = oldFilePath,
                                                                                    ChangeType = changeType
                                                                                });
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            _logger.LogError(ex, "Exception in FileChanged event subscribers for {FilePath}", filePath);
                                                        }
                                                    });
        }
        finally
        {
            _disposeLock.ExitReadLock();
        }
    }

    private static bool IsValidFileExtension(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLowerInvariant();
        return extension is ".vmx" or ".vbox" or ".log" or ".vmss";
    }
}