namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <summary>
///     Retry policy for accessing files that may be locked or temporarily inaccessible.
///     Implements exponential backoff: 100ms, 200ms, 400ms delays between retries.
/// </summary>
public interface IFileAccessRetryPolicy
{
    /// <summary>
    ///     Attempts to execute an action with automatic retry on file access failures.
    /// </summary>
    /// <param name="action">The action to execute (e.g., read file, parse file).</param>
    /// <param name="filePath">The file path for logging purposes.</param>
    /// <param name="onRetry">Callback invoked when a retry occurs, with retry count and exception.</param>
    /// <returns>True if action succeeded, false if all retries were exhausted.</returns>
    Task<bool> TryExecuteAsync(
        Func<Task> action,
        string filePath,
        Func<int, Exception, Task> onRetry);

    /// <summary>
    ///     Attempts to execute a function with automatic retry on file access failures.
    /// </summary>
    /// <typeparam name="T">The return type of the function.</typeparam>
    /// <param name="function">The function to execute.</param>
    /// <param name="filePath">The file path for logging purposes.</param>
    /// <param name="onRetry">Callback invoked when a retry occurs, with retry count and exception.</param>
    /// <returns>A tuple of (success, result). Result is default(T) if unsuccessful.</returns>
    Task<(bool Success, T Result)> TryExecuteAsync<T>(
        Func<Task<T>> function,
        string filePath,
        Func<int, Exception, Task> onRetry);
}