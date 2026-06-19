using Microsoft.Extensions.Logging;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <inheritdoc cref="IVmFileChangeHandler" />
public class VmFileChangeHandler(
    [NotNull] IVmFileWatcher vmFileWatcher,
    [NotNull] ILoad load,
    [NotNull] IHandleMachineFromPath handleMachineFromPath,
    [NotNull] IResolveMachinePoolPath resolveMachinePoolPath,
    [NotNull] ILogger<VmFileChangeHandler> logger,
    [NotNull] IFileAccessRetryPolicy fileAccessRetryPolicy,
    [NotNull] IUpdateMachineCollection updateMachineCollection,
    [NotNull] IUpdateMachineStateFromFile updateMachineStateFromFile) : IVmFileChangeHandler
{
    private readonly IVmFileWatcher _vmFileWatcher =
        vmFileWatcher ?? throw new ArgumentNullException(nameof(vmFileWatcher));

    private readonly ILoad _load = load ?? throw new ArgumentNullException(nameof(load));

    private readonly IHandleMachineFromPath _handleMachineFromPath =
        handleMachineFromPath ?? throw new ArgumentNullException(nameof(handleMachineFromPath));

    private readonly IResolveMachinePoolPath _resolveMachinePoolPath =
        resolveMachinePoolPath ?? throw new ArgumentNullException(nameof(resolveMachinePoolPath));

    private readonly ILogger<VmFileChangeHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly IFileAccessRetryPolicy _fileAccessRetryPolicy =
        fileAccessRetryPolicy ?? throw new ArgumentNullException(nameof(fileAccessRetryPolicy));

    private readonly IUpdateMachineCollection _updateMachineCollection =
        updateMachineCollection ?? throw new ArgumentNullException(nameof(updateMachineCollection));

    private readonly IUpdateMachineStateFromFile _updateMachineStateFromFile =
        updateMachineStateFromFile ?? throw new ArgumentNullException(nameof(updateMachineStateFromFile));

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

            if (extension is ".log" or ".vmss")
            {
                _logger.LogDebug("Handling state/log update for {FilePath}", filePath);
                _updateMachineStateFromFile.UpdateFor(filePath, loadValue);
                return;
            }

            var machinePoolPath = _resolveMachinePoolPath.ValueFor(filePath);
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
            catch (DirectoryNotFoundException ex)
            {
                _logger.LogDebug(ex, "Transient directory not found for {FilePath}, will retry", filePath);
                _ = RetryMachineLoadAsync(machinePath, filePath, loadValue);
                return;
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                _logger.LogWarning(ex, "File access error for {FilePath}, will retry", filePath);
                _ = RetryMachineLoadAsync(machinePath, filePath, loadValue);
                return;
            }

            if (machine is null)
            {
                _logger.LogDebug("Failed to parse machine from {FilePath}", filePath);
                return;
            }

            _updateMachineCollection.ReplaceByPath(loadValue, filePath, machine);
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

            _updateMachineCollection.ReplaceByPath(loadValue, filePath, machine);
            _logger.LogInformation("Machine successfully updated after retry for {FilePath}", filePath);
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

            if (extension is ".log" or ".vmss")
            {
                _logger.LogDebug("Handling state/log update for deleted file {FilePath}", filePath);
                _updateMachineStateFromFile.UpdateFor(filePath, loadValue);
                return;
            }

            _updateMachineCollection.RemoveByPath(loadValue, filePath);
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

            if (extension is ".log" or ".vmss")
            {
                _logger.LogDebug("Handling state/log update for renamed file {OldPath} -> {NewPath}", oldFilePath,
                    newFilePath);
                _updateMachineStateFromFile.UpdateFor(newFilePath, loadValue);
                return;
            }

            var machinePoolPath = _resolveMachinePoolPath.ValueFor(newFilePath);
            if (machinePoolPath is null)
            {
                _logger.LogDebug("No machine pool path found for renamed file {OldPath} -> {NewPath}", oldFilePath,
                    newFilePath);
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
            catch (DirectoryNotFoundException ex)
            {
                _logger.LogDebug(ex, "Transient directory not found for renamed file {NewPath}, will retry",
                    newFilePath);
                _ = RetryMachineRenameAsync(oldFilePath, machinePath, newFilePath, loadValue);
                return;
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                _logger.LogWarning(ex, "File access error for renamed file {NewPath}, will retry", newFilePath);
                _ = RetryMachineRenameAsync(oldFilePath, machinePath, newFilePath, loadValue);
                return;
            }

            _updateMachineCollection.RemoveByPath(loadValue, oldFilePath);

            if (machine is not null)
            {
                _updateMachineCollection.ReplaceByPath(loadValue, newFilePath, machine);
                _logger.LogDebug("Machine renamed in UI: {OldPath} -> {NewPath}", oldFilePath, newFilePath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in HandleRenamed for {OldPath} -> {NewPath}", oldFilePath, newFilePath);
        }
    }

    private async Task RetryMachineRenameAsync(string oldFilePath, MachinePath machinePath, string newFilePath,
                                               LoadHelper loadValue)
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
                _logger.LogWarning("Failed to load renamed machine after retries: {OldPath} -> {NewPath}", oldFilePath,
                    newFilePath);
                return;
            }

            _updateMachineCollection.RemoveByPath(loadValue, oldFilePath);
            _updateMachineCollection.ReplaceByPath(loadValue, newFilePath, machine);
            _logger.LogInformation("Machine successfully updated after retry for rename: {OldPath} -> {NewPath}",
                oldFilePath, newFilePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in RetryMachineRenameAsync for {OldPath} -> {NewPath}", oldFilePath,
                newFilePath);
        }
    }
}