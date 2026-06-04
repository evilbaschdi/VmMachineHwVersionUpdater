using Avalonia.Threading;
using Microsoft.Extensions.Logging;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <inheritdoc cref="IVmFileChangeHandler" />
public class VmFileChangeHandler(
    [NotNull] IVmFileWatcher vmFileWatcher,
    [NotNull] ILoad load,
    [NotNull] IHandleMachineFromPath handleMachineFromPath,
    [NotNull] IPathSettings pathSettings,
    [NotNull] ILogger<VmFileChangeHandler> logger,
    [NotNull] IFileAccessRetryPolicy fileAccessRetryPolicy,
    [NotNull] IReadLogInformation readLogInformation,
    [NotNull] ISetMachineIsEnabledForEditing setMachineIsEnabledForEditing,
    [NotNull] ISetExtendedInformation setExtendedInformation) : IVmFileChangeHandler
{
    private readonly IVmFileWatcher _vmFileWatcher = vmFileWatcher ?? throw new ArgumentNullException(nameof(vmFileWatcher));
    private readonly ILoad _load = load ?? throw new ArgumentNullException(nameof(load));
    private readonly IHandleMachineFromPath _handleMachineFromPath = handleMachineFromPath ?? throw new ArgumentNullException(nameof(handleMachineFromPath));
    private readonly IPathSettings _pathSettings = pathSettings ?? throw new ArgumentNullException(nameof(pathSettings));
    private readonly ILogger<VmFileChangeHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IFileAccessRetryPolicy _fileAccessRetryPolicy = fileAccessRetryPolicy ?? throw new ArgumentNullException(nameof(fileAccessRetryPolicy));
    private readonly IReadLogInformation _readLogInformation = readLogInformation ?? throw new ArgumentNullException(nameof(readLogInformation));

    private readonly ISetMachineIsEnabledForEditing _setMachineIsEnabledForEditing =
        setMachineIsEnabledForEditing ?? throw new ArgumentNullException(nameof(setMachineIsEnabledForEditing));

    private readonly ISetExtendedInformation _setExtendedInformation = setExtendedInformation ?? throw new ArgumentNullException(nameof(setExtendedInformation));

    private bool _disposed;

    /// <inheritdoc />
    public void Start()
    {
        _logger.LogDebug("Starting VmFileChangeHandler");
        _vmFileWatcher.FileChanged += OnFileChanged;
        _vmFileWatcher.Start();
        _logger.LogInformation("VmFileChangeHandler started successfully");
    }

    /// <inheritdoc />
    public void Stop()
    {
        _logger.LogDebug("Stopping VmFileChangeHandler");
        _vmFileWatcher.FileChanged -= OnFileChanged;
        _vmFileWatcher.Stop();
        _logger.LogInformation("VmFileChangeHandler stopped");
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

        try
        {
            _logger.LogDebug("File changed event received: {FilePath} ({ChangeType})", args.FilePath, args.ChangeType);

            var loadValue = _load.Value;
            if (loadValue?.VmDataGridItemsSource is null)
            {
                _logger.LogWarning("Load value or VmDataGridItemsSource is null for file {FilePath}", args.FilePath);
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in OnFileChanged handler for {FilePath}", args.FilePath);
        }
    }

    private void HandleChangedOrCreated(string filePath, LoadHelper loadValue)
    {
        try
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();

            // Handle .log and .vmss files - update state and log info in UI
            if (extension is ".log" or ".vmss")
            {
                _logger.LogDebug("Handling state/log update for {FilePath}", filePath);
                UpdateMachineStateAndLogInfo(filePath, loadValue);
                return;
            }

            var machinePoolPath = ResolveMachinePoolPath(filePath);
            if (machinePoolPath is null)
            {
                _logger.LogDebug("No machine pool path found for {FilePath}", filePath);
                return;
            }

            var machinePath = new MachinePath
                              {
                                  MachinePoolPath = machinePoolPath,
                                  MachineFilePath = filePath
                              };

            Machine machine;
            try
            {
                machine = _handleMachineFromPath.ValueFor(machinePath);
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                _logger.LogWarning(ex, "File access error for {FilePath}, will retry", filePath);
                // Attempt retry asynchronously
                _ = RetryMachineLoadAsync(machinePath, filePath, loadValue);
                return;
            }

            if (machine is null)
            {
                _logger.LogDebug("Failed to parse machine from {FilePath}", filePath);
                return;
            }

            Dispatcher.UIThread.Post(() =>
                                     {
                                         try
                                         {
                                             var existing = loadValue.VmDataGridItemsSource.FirstOrDefault(m =>
                                                                                                               string.Equals(m.Path, filePath, StringComparison.OrdinalIgnoreCase));

                                             if (existing is not null)
                                             {
                                                 loadValue.VmDataGridItemsSource.Remove(existing);
                                             }

                                             loadValue.VmDataGridItemsSource.Add(machine);
                                             _logger.LogDebug("Machine updated in UI for {FilePath}", filePath);
                                         }
                                         catch (Exception ex)
                                         {
                                             _logger.LogError(ex, "Error updating UI for {FilePath}", filePath);
                                         }
                                     });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in HandleChangedOrCreated for {FilePath}", filePath);
        }
    }

    private async Task RetryMachineLoadAsync(MachinePath machinePath, string filePath, LoadHelper loadValue)
    {
        try
        {
            var (success, machine) = await _fileAccessRetryPolicy.TryExecuteAsync(
                async () =>
                {
                    await Task.Yield();
                    return _handleMachineFromPath.ValueFor(machinePath);
                },
                filePath,
                async (attempt, ex) =>
                {
                    _logger.LogDebug(ex, "Retry attempt {Attempt} for {FilePath}", attempt, filePath);
                    await Task.CompletedTask;
                });

            if (!success || machine is null)
            {
                _logger.LogWarning("Failed to load machine after retries for {FilePath}", filePath);
                return;
            }

            Dispatcher.UIThread.Post(() =>
                                     {
                                         try
                                         {
                                             var existing = loadValue.VmDataGridItemsSource.FirstOrDefault(m =>
                                                                                                               string.Equals(m.Path, filePath, StringComparison.OrdinalIgnoreCase));

                                             if (existing is not null)
                                             {
                                                 loadValue.VmDataGridItemsSource.Remove(existing);
                                             }

                                             loadValue.VmDataGridItemsSource.Add(machine);
                                             _logger.LogInformation("Machine successfully updated after retry for {FilePath}", filePath);
                                         }
                                         catch (Exception ex)
                                         {
                                             _logger.LogError(ex, "Error updating UI after retry for {FilePath}", filePath);
                                         }
                                     });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in RetryMachineLoadAsync for {FilePath}", filePath);
        }
    }

    private void HandleDeleted(string filePath, LoadHelper loadValue)
    {
        try
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();

            // Handle .log and .vmss file deletions - refresh state/log info
            if (extension is ".log" or ".vmss")
            {
                _logger.LogDebug("Handling state/log update for deleted file {FilePath}", filePath);
                UpdateMachineStateAndLogInfo(filePath, loadValue);
                return;
            }

            Dispatcher.UIThread.Post(() =>
                                     {
                                         try
                                         {
                                             var existing = loadValue.VmDataGridItemsSource.FirstOrDefault(m =>
                                                                                                               string.Equals(m.Path, filePath, StringComparison.OrdinalIgnoreCase));

                                             if (existing is not null)
                                             {
                                                 loadValue.VmDataGridItemsSource.Remove(existing);
                                                 _logger.LogDebug("Machine removed from UI for {FilePath}", filePath);
                                             }
                                         }
                                         catch (Exception ex)
                                         {
                                             _logger.LogError(ex, "Error removing machine from UI for {FilePath}", filePath);
                                         }
                                     });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in HandleDeleted for {FilePath}", filePath);
        }
    }

    private void HandleRenamed(string oldFilePath, string newFilePath, LoadHelper loadValue)
    {
        try
        {
            var extension = Path.GetExtension(newFilePath).ToLowerInvariant();

            // Handle .log and .vmss file renames - update state/log info
            if (extension is ".log" or ".vmss")
            {
                _logger.LogDebug("Handling state/log update for renamed file {OldPath} -> {NewPath}", oldFilePath, newFilePath);
                UpdateMachineStateAndLogInfo(newFilePath, loadValue);
                return;
            }

            var machinePoolPath = ResolveMachinePoolPath(newFilePath);
            if (machinePoolPath is null)
            {
                _logger.LogDebug("No machine pool path found for renamed file {OldPath} -> {NewPath}", oldFilePath, newFilePath);
                return;
            }

            var machinePath = new MachinePath
                              {
                                  MachinePoolPath = machinePoolPath,
                                  MachineFilePath = newFilePath
                              };

            Machine machine;
            try
            {
                machine = _handleMachineFromPath.ValueFor(machinePath);
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                _logger.LogWarning(ex, "File access error for renamed file {NewPath}, will retry", newFilePath);
                _ = RetryMachineRenameAsync(oldFilePath, machinePath, newFilePath, loadValue);
                return;
            }

            Dispatcher.UIThread.Post(() =>
                                     {
                                         try
                                         {
                                             var existing = loadValue.VmDataGridItemsSource.FirstOrDefault(m =>
                                                                                                               string.Equals(m.Path, oldFilePath,
                                                                                                                   StringComparison.OrdinalIgnoreCase));

                                             if (existing is not null)
                                             {
                                                 loadValue.VmDataGridItemsSource.Remove(existing);
                                             }

                                             if (machine is null)
                                             {
                                                 return;
                                             }

                                             loadValue.VmDataGridItemsSource.Add(machine);
                                             _logger.LogDebug("Machine renamed in UI: {OldPath} -> {NewPath}", oldFilePath, newFilePath);
                                         }
                                         catch (Exception ex)
                                         {
                                             _logger.LogError(ex, "Error updating UI for renamed file {OldPath} -> {NewPath}", oldFilePath, newFilePath);
                                         }
                                     });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in HandleRenamed for {OldPath} -> {NewPath}", oldFilePath, newFilePath);
        }
    }

    private async Task RetryMachineRenameAsync(string oldFilePath, MachinePath machinePath, string newFilePath, LoadHelper loadValue)
    {
        try
        {
            var (success, machine) = await _fileAccessRetryPolicy.TryExecuteAsync(
                async () =>
                {
                    await Task.Yield();
                    return _handleMachineFromPath.ValueFor(machinePath);
                },
                newFilePath,
                async (attempt, ex) =>
                {
                    _logger.LogDebug(ex, "Retry attempt {Attempt} for renamed file {NewPath}", attempt, newFilePath);
                    await Task.CompletedTask;
                });

            if (!success || machine is null)
            {
                _logger.LogWarning("Failed to load renamed machine after retries: {OldPath} -> {NewPath}", oldFilePath, newFilePath);
                return;
            }

            Dispatcher.UIThread.Post(() =>
                                     {
                                         try
                                         {
                                             var existing = loadValue.VmDataGridItemsSource.FirstOrDefault(m =>
                                                                                                               string.Equals(m.Path, oldFilePath,
                                                                                                                   StringComparison.OrdinalIgnoreCase));

                                             if (existing is not null)
                                             {
                                                 loadValue.VmDataGridItemsSource.Remove(existing);
                                             }

                                             loadValue.VmDataGridItemsSource.Add(machine);
                                             _logger.LogInformation("Machine successfully updated after retry for rename: {OldPath} -> {NewPath}", oldFilePath, newFilePath);
                                         }
                                         catch (Exception ex)
                                         {
                                             _logger.LogError(ex, "Error updating UI after retry for renamed file {OldPath} -> {NewPath}", oldFilePath, newFilePath);
                                         }
                                     });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in RetryMachineRenameAsync for {OldPath} -> {NewPath}", oldFilePath, newFilePath);
        }
    }

    private string ResolveMachinePoolPath(string filePath)
    {
        var allPaths = _pathSettings.VmPool.Concat(_pathSettings.ArchivePath);

        return allPaths
               .Where(pool => filePath.StartsWith(pool, StringComparison.OrdinalIgnoreCase))
               .OrderByDescending(pool => pool.Length)
               .FirstOrDefault();
    }

    private void UpdateMachineStateAndLogInfo(string filePath, LoadHelper loadValue)
    {
        try
        {
            // Determine if this is a .log or .vmss file change
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            var machineDir = Path.GetDirectoryName(filePath);

            if (string.IsNullOrEmpty(machineDir))
            {
                _logger.LogDebug("Cannot determine machine directory for {FilePath}", filePath);
                return;
            }

            // Find the machine by directory path
            var machine = loadValue.VmDataGridItemsSource.FirstOrDefault(m =>
                                                                             string.Equals(Path.GetDirectoryName(m.Path), machineDir, StringComparison.OrdinalIgnoreCase));

            if (machine is null)
            {
                _logger.LogDebug("Machine not found for directory {Directory}", machineDir);
                return;
            }

            // Update on UI thread
            Dispatcher.UIThread.Post(() =>
                                     {
                                         try
                                         {
                                             switch (extension)
                                             {
                                                 case ".vmss":
                                                 {
                                                     // Update machine state based on .vmss file presence
                                                     var vmssFiles = Directory.GetFiles(machineDir, "*.vmss");
                                                     var newState = vmssFiles.Length > 0 ? MachineState.Paused : MachineState.Off;
                                                     machine.MachineState = newState;

                                                     // Update editability and icons
                                                     _setMachineIsEnabledForEditing.RunFor(machine);
                                                     var rawMachine = new RawMachine
                                                                      {
                                                                          Annotation = machine.Annotation,
                                                                          ManagedVmAutoAddVTpm = machine.ManagedVmAutoAddVTpm
                                                                      };
                                                     _setExtendedInformation.RunFor(rawMachine, machine);

                                                     _logger.LogDebug("Updated MachineState to {State} for {Directory}", newState, machineDir);
                                                     break;
                                                 }
                                                 case ".log":
                                                     // Update log info from vmware.log
                                                     var (logDate, logDiff) = _readLogInformation.ValueFor(machineDir);
                                                     machine.LogLastDate = logDate;
                                                     machine.LogLastDateDiff = logDiff;
                                                     _logger.LogDebug("Updated LogLastDate={LogDate} for {Directory}", logDate, machineDir);
                                                     break;
                                             }
                                         }
                                         catch (Exception ex)
                                         {
                                             _logger.LogError(ex, "Error updating machine state/log for {Directory}", machineDir);
                                         }
                                     });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in UpdateMachineStateAndLogInfo for {FilePath}", filePath);
        }
    }
}