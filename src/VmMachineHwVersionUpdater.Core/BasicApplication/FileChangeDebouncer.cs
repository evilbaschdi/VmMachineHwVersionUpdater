using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <inheritdoc cref="IFileChangeDebouncer" />
public class FileChangeDebouncer(
    [NotNull] ILogger<FileChangeDebouncer> logger) : IFileChangeDebouncer
{
    private readonly ILogger<FileChangeDebouncer> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly ConcurrentDictionary<string, CancellationTokenSource> _debounceTokens =
        new(StringComparer.OrdinalIgnoreCase);

    private readonly TimeSpan _debounceInterval = TimeSpan.FromMilliseconds(1000);
    private bool _disposed;

    /// <inheritdoc />
    public void Debounce(string key, Action callback)
    {
        ArgumentNullException.ThrowIfNull(key);
        ArgumentNullException.ThrowIfNull(callback);

        if (_disposed)
        {
            return;
        }

        if (_debounceTokens.TryRemove(key, out var existingCts))
        {
            _logger.LogDebug("Cancelling previous debounce for {Key}", key);
            try
            {
                existingCts.Cancel();
                existingCts.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling previous debounce token for {Key}", key);
            }
        }

        var cts = new CancellationTokenSource();
        var token = cts.Token;
        _debounceTokens[key] = cts;

        _logger.LogDebug("Debouncing event for {Key}", key);

        _ = Task.Delay(_debounceInterval, token).ContinueWith(
            _ =>
            {
                try
                {
                    if (_debounceTokens.TryRemove(key, out var removedCts))
                    {
                        try
                        {
                            removedCts.Dispose();
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error disposing debounce token for {Key}", key);
                        }
                    }

                    try
                    {
                        _logger.LogDebug("Executing debounced callback for {Key}", key);
                        callback();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Exception in debounced callback for {Key}", key);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error in debounce continuation for {Key}", key);
                }
            },
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnRanToCompletion,
            TaskScheduler.Default);
    }

    /// <inheritdoc />
    public void CancelAll()
    {
        foreach (var cts in _debounceTokens.Values)
        {
            try
            {
                cts.Cancel();
                cts.Dispose();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error disposing CancellationTokenSource during CancelAll");
            }
        }

        _debounceTokens.Clear();
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        CancelAll();
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}