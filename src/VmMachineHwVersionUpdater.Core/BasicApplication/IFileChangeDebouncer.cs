namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <summary>
///     Debounces file change events by key, ensuring only the last event within the debounce window fires.
/// </summary>
public interface IFileChangeDebouncer : IDisposable
{
    /// <summary>
    ///     Debounces the specified <paramref name="callback" /> by <paramref name="key" />.
    ///     If called again with the same key before the debounce interval elapses, the previous call is cancelled.
    /// </summary>
    void Debounce(string key, Action callback);

    /// <summary>
    ///     Cancels all pending debounced callbacks and clears state.
    /// </summary>
    void CancelAll();
}