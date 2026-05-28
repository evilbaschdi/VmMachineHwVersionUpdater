namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <summary>
///     Watches configured VmPool directories for .vmx/.vbox file changes.
/// </summary>
public interface IVmFileWatcher : IDisposable
{
    /// <summary>
    ///     Starts monitoring all configured VmPool and Archive directories.
    /// </summary>
    void Start();

    /// <summary>
    ///     Stops monitoring and disposes all file system watchers.
    /// </summary>
    void Stop();

    /// <summary>
    ///     Raised when a .vmx or .vbox file is changed, created, deleted, or renamed (debounced).
    /// </summary>
    event Action<VmFileChangedEventArgs> FileChanged;
}