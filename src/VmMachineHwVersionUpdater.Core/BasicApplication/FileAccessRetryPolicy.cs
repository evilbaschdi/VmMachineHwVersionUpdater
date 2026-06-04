namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <inheritdoc />
public class FileAccessRetryPolicy : IFileAccessRetryPolicy
{
    private const int MaxRetries = 3;

    private static readonly TimeSpan[] Delays =
    [
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
            // ReSharper disable once RedundantCatchClause
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
            // ReSharper disable once RedundantCatchClause
            catch
            {
                // Non-file-access exception, rethrow
                throw;
            }
        }

        return (Success: false, Result: default);
    }

    /// <summary>
    ///     Determines if an exception is related to file access issues (lock, access denied, not found during transition).
    /// </summary>
    private static bool IsFileAccessException(Exception ex) => ex is IOException or UnauthorizedAccessException;
}