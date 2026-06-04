using Microsoft.Extensions.Logging;

namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <summary>
/// Generic configuration for file-based logging that can be reused across different applications and libraries.
/// </summary>
public class FileLoggerConfiguration
{
    /// <summary>
    /// Gets or sets the base directory where logs will be stored.
    /// Defaults to {AppContext.BaseDirectory}/logs
    /// </summary>
    public string LogDirectory { get; set; } = Path.Combine(AppContext.BaseDirectory, "logs");

    /// <summary>
    /// Gets or sets the log file name pattern.
    /// Use {date} as a placeholder for the current date (yyyy-MM-dd format).
    /// Example: "myapp-{date}.log" becomes "myapp-2026-06-04.log"
    /// </summary>
    public string LogFileNamePattern { get; set; } = "app-{date}.log";

    /// <summary>
    /// Gets or sets the number of days to retain log files.
    /// Log files older than this will be automatically deleted.
    /// </summary>
    public int LogRetentionDays { get; set; } = 7;

    /// <summary>
    /// Gets or sets the minimum log level.
    /// </summary>
    public LogLevel MinimumLogLevel { get; set; } = LogLevel.Debug;

    /// <summary>
    /// Gets the log directory path, ensuring it exists.
    /// Cleans up old log files based on retention policy.
    /// </summary>
    public string GetLogDirectory()
    {
        if (!Directory.Exists(LogDirectory))
        {
            Directory.CreateDirectory(LogDirectory);
        }

        CleanOldLogs();
        return LogDirectory;
    }

    /// <summary>
    /// Gets the full path to the current log file.
    /// </summary>
    public string GetLogFilePath()
    {
        var logDir = GetLogDirectory();
        var logFileName = LogFileNamePattern.Replace("{date}", DateTime.Now.ToString("yyyy-MM-dd"));
        return Path.Combine(logDir, logFileName);
    }

    /// <summary>
    /// Cleans up log files older than the retention period.
    /// </summary>
    private void CleanOldLogs()
    {
        try
        {
            var logDir = new DirectoryInfo(LogDirectory);
            var cutoffDate = DateTime.Now.AddDays(-LogRetentionDays);

            // Extract the pattern prefix (everything before {date})
            var patternPrefix = LogFileNamePattern.Split('{')[0];
            var searchPattern = $"{patternPrefix}*.log";

            foreach (var file in logDir.GetFiles(searchPattern))
            {
                if (file.LastWriteTime < cutoffDate)
                {
                    file.Delete();
                }
            }
        }
        catch
        {
            // Silently fail log cleanup to avoid breaking the app
        }
    }

    /// <summary>
    /// Configures file-based logging using this configuration.
    /// </summary>
    public void Configure(ILoggingBuilder builder)
    {
        builder.ClearProviders();
        builder.SetMinimumLevel(MinimumLogLevel);
        builder.AddProvider(new SimpleFileLoggerProvider(GetLogFilePath(), MinimumLogLevel));
    }
}

/// <summary>
/// Simple file-based logging provider.
/// Can be used standalone or via FileLoggerConfiguration.
/// </summary>
internal sealed class SimpleFileLoggerProvider(string logFilePath, LogLevel minimumLogLevel = LogLevel.Debug) : ILoggerProvider
{
    private readonly string _logFilePath = logFilePath ?? throw new ArgumentNullException(nameof(logFilePath));
    private readonly LogLevel _minimumLogLevel = minimumLogLevel;
    private readonly object _lockObject = new object();

    /// <inheritdoc />
    public ILogger CreateLogger(string categoryName)
    {
        return new SimpleFileLogger(_logFilePath, _minimumLogLevel, _lockObject);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        // Nothing to dispose
    }
}

/// <summary>
/// Simple file logger that writes to disk with thread-safe access.
/// </summary>
internal sealed class SimpleFileLogger(string logFilePath, LogLevel minimumLogLevel, object lockObject) : ILogger
{
    private readonly string _logFilePath = logFilePath ?? throw new ArgumentNullException(nameof(logFilePath));
    private readonly LogLevel _minimumLogLevel = minimumLogLevel;
    private readonly object _lockObject = lockObject ?? throw new ArgumentNullException(nameof(lockObject));

    /// <inheritdoc />
    public IDisposable BeginScope<TState>(TState state)
        where TState : notnull
    {
        return NullScope.Instance;
    }

    /// <inheritdoc />
    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= _minimumLogLevel;
    }

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
