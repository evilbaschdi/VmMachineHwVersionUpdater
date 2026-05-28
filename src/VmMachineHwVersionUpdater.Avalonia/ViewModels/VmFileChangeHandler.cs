using Avalonia.Threading;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <inheritdoc cref="IVmFileChangeHandler" />
public class VmFileChangeHandler(
    [NotNull] IVmFileWatcher vmFileWatcher,
    [NotNull] ILoad load,
    [NotNull] IHandleMachineFromPath handleMachineFromPath,
    [NotNull] IPathSettings pathSettings) : IVmFileChangeHandler
{
    private readonly IVmFileWatcher _vmFileWatcher =
        vmFileWatcher ?? throw new ArgumentNullException(nameof(vmFileWatcher));

    private readonly ILoad _load = load ?? throw new ArgumentNullException(nameof(load));

    private readonly IHandleMachineFromPath _handleMachineFromPath =
        handleMachineFromPath ?? throw new ArgumentNullException(nameof(handleMachineFromPath));

    private readonly IPathSettings
        _pathSettings = pathSettings ?? throw new ArgumentNullException(nameof(pathSettings));

    private bool _disposed;

    /// <inheritdoc />
    public void Start()
    {
        _vmFileWatcher.FileChanged += OnFileChanged;
        _vmFileWatcher.Start();
    }

    /// <inheritdoc />
    public void Stop()
    {
        _vmFileWatcher.FileChanged -= OnFileChanged;
        _vmFileWatcher.Stop();
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

    private void OnFileChanged(VmFileChangedEventArgs args)
    {
        ArgumentNullException.ThrowIfNull(args);

        var loadValue = _load.Value;
        if (loadValue?.VmDataGridItemsSource is null)
        {
            return;
        }

        switch (args.ChangeType)
        {
            case VmFileChangeType.Changed:
            case VmFileChangeType.Created:
                HandleChangedOrCreated(args.FilePath, loadValue);
                break;

            case VmFileChangeType.Deleted:
                HandleDeleted(args.FilePath, loadValue);
                break;

            case VmFileChangeType.Renamed:
                HandleRenamed(args.OldFilePath, args.FilePath, loadValue);
                break;
        }
    }

    private void HandleChangedOrCreated(string filePath, LoadHelper loadValue)
    {
        var machinePoolPath = ResolveMachinePoolPath(filePath);
        if (machinePoolPath is null)
        {
            return;
        }

        var machinePath = new MachinePath
                          {
                              MachinePoolPath = machinePoolPath,
                              MachineFilePath = filePath
                          };

        var machine = _handleMachineFromPath.ValueFor(machinePath);
        if (machine is null)
        {
            return;
        }

        Dispatcher.UIThread.Post(() =>
                                 {
                                     var existing = loadValue.VmDataGridItemsSource.FirstOrDefault(m =>
                                                                                                       string.Equals(m.Path, filePath, StringComparison.OrdinalIgnoreCase));

                                     if (existing is not null)
                                     {
                                         loadValue.VmDataGridItemsSource.Remove(existing);
                                     }

                                     loadValue.VmDataGridItemsSource.Add(machine);
                                 });
    }

    private void HandleDeleted(string filePath, LoadHelper loadValue)
    {
        Dispatcher.UIThread.Post(() =>
                                 {
                                     var existing = loadValue.VmDataGridItemsSource.FirstOrDefault(m =>
                                                                                                       string.Equals(m.Path, filePath, StringComparison.OrdinalIgnoreCase));

                                     if (existing is not null)
                                     {
                                         loadValue.VmDataGridItemsSource.Remove(existing);
                                     }
                                 });
    }

    private void HandleRenamed(string oldFilePath, string newFilePath, LoadHelper loadValue)
    {
        var machinePoolPath = ResolveMachinePoolPath(newFilePath);
        if (machinePoolPath is null)
        {
            return;
        }

        var machinePath = new MachinePath
                          {
                              MachinePoolPath = machinePoolPath,
                              MachineFilePath = newFilePath
                          };

        var machine = _handleMachineFromPath.ValueFor(machinePath);

        Dispatcher.UIThread.Post(() =>
                                 {
                                     var existing = loadValue.VmDataGridItemsSource.FirstOrDefault(m =>
                                                                                                       string.Equals(m.Path, oldFilePath, StringComparison.OrdinalIgnoreCase));

                                     if (existing is not null)
                                     {
                                         loadValue.VmDataGridItemsSource.Remove(existing);

                                         if (machine is not null)
                                         {
                                             loadValue.VmDataGridItemsSource.Add(machine);
                                         }
                                     }
                                     else if (machine is not null)
                                     {
                                         loadValue.VmDataGridItemsSource.Add(machine);
                                     }
                                 });
    }

    private string ResolveMachinePoolPath(string filePath)
    {
        var allPaths = _pathSettings.VmPool.Concat(_pathSettings.ArchivePath);

        return allPaths
               .Where(pool => filePath.StartsWith(pool, StringComparison.OrdinalIgnoreCase))
               .OrderByDescending(pool => pool.Length)
               .FirstOrDefault();
    }
}