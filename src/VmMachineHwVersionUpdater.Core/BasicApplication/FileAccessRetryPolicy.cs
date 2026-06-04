namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <summary>
/// Retry policy for accessing files that may be locked or temporarily inaccessible.
/// Implements exponential backoff: 100ms, 200ms, 400ms delays between retries.
/// </summary>
public interface IFileAccessRetryPolicy
{
    /// <summary>
    /// Attempts to execute an action with automatic retry on file access failures.
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
    /// Attempts to execute a function with automatic retry on file access failures.
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

/// <inheritdoc />
public class FileAccessRetryPolicy : IFileAccessRetryPolicy
{
    private const int MaxRetries = 3;
    private static readonly TimeSpan[] Delays = [
        TimeSpan.FromMilliseconds(100),
        TimeSpan.FromMilliseconds(200),
        TimeSpan.FromMilliseconds(400)
    ];

    /// <inheritdoc />
    public async Task<bool> TryExecuteAsync(
        Func<Task> action,
        string filePath,
        Func<int, Exception, Task> onRetry)
    {
        ArgumentNullException.ThrowIfNull(action);
        ArgumentNullException.ThrowIfNull(filePath);
        onRetry ??= (_, _) => Task.CompletedTask;

        for (var attempt = 0; attempt <= MaxRetries; attempt++)
        {
            try
            {
                await action();
                return true;
            }
            catch (Exception ex) when (IsFileAccessException(ex) && attempt < MaxRetries)
            {
                await onRetry(attempt + 1, ex);
                await Task.Delay(Delays[attempt]);
            }
            catch
            {
                // Non-file-access exception, rethrow
                throw;
            }
        }

        return false;
    }

    /// <inheritdoc />
    public async Task<(bool Success, T Result)> TryExecuteAsync<T>(
        Func<Task<T>> function,
        string filePath,
        Func<int, Exception, Task> onRetry)
    {
        ArgumentNullException.ThrowIfNull(function);
        ArgumentNullException.ThrowIfNull(filePath);
        onRetry ??= (_, _) => Task.CompletedTask;

        for (var attempt = 0; attempt <= MaxRetries; attempt++)
        {
            try
            {
                var result = await function();
                return (Success: true, Result: result);
            }
            catch (Exception ex) when (IsFileAccessException(ex) && attempt < MaxRetries)
            {
                await onRetry(attempt + 1, ex);
                await Task.Delay(Delays[attempt]);
            }
            catch
            {
                // Non-file-access exception, rethrow
                throw;
            }
        }

        return (Success: false, Result: default);
    }

    /// <summary>
    /// Determines if an exception is related to file access issues (lock, access denied, not found during transition).
    /// </summary>
    private static bool IsFileAccessException(Exception ex)
    {
        return ex is IOException or UnauthorizedAccessException;
    }
}
