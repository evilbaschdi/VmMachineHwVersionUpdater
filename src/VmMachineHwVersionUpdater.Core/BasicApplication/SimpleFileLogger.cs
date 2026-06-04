using Microsoft.Extensions.Logging;

namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <summary>
///     Simple file logger that writes to disk with thread-safe access.
/// </summary>
internal sealed class SimpleFileLogger(string logFilePath, LogLevel minimumLogLevel, object lockObject) : ILogger
{
    private readonly string _logFilePath = logFilePath ?? throw new ArgumentNullException(nameof(logFilePath));
    private readonly LogLevel _minimumLogLevel = minimumLogLevel;
    private readonly object _lockObject = lockObject ?? throw new ArgumentNullException(nameof(lockObject));

    /// <inheritdoc />
    public IDisposable BeginScope<TState>(TState state)
        where TState : notnull =>
        NullScope.Instance;

    /// <inheritdoc />
    public bool IsEnabled(LogLevel logLevel) => logLevel >= _minimumLogLevel;

    /// <inheritdoc />
    public void Log<TState>(
        LogLevel logLevel,
        EventId eventId,
        TState state,
        Exception exception,
        Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        var message = formatter(state, exception);
        var logEntry = FormatLogEntry(logLevel, message, exception);

        lock (_lockObject)
        {
            try
            {
                File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
            }
            catch
            {
                // Silently fail to avoid breaking the app
            }
        }
    }

    private static string FormatLogEntry(LogLevel logLevel, string message, Exception exception)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        var level = logLevel.ToString().ToUpperInvariant().PadRight(5);
        var entry = $"[{timestamp}] {level} {message}";

        if (exception is not null)
        {
            entry += Environment.NewLine + exception;
        }

        return entry;
    }

    private sealed class NullScope : IDisposable
    {
        public static readonly NullScope Instance = new();

        public void Dispose()
        {
            // Nothing to dispose
        }
    }
}