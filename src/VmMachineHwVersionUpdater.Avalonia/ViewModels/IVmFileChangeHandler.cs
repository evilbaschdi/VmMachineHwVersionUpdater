namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <summary>
///     Handles file system change events and updates the machine collection on the UI thread.
/// </summary>
public interface IVmFileChangeHandler : IDisposable
{
    /// <summary>
    ///     Subscribes to IVmFileWatcher events and starts handling file changes.
    /// </summary>
    void Start();

    /// <summary>
    ///     Unsubscribes from events and stops handling file changes.
    /// </summary>
    void Stop();
}