using Microsoft.Extensions.Logging;

namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <summary>
/// Watcher-specific configuration for file-based logging.
/// Provides convenient defaults for VM file watcher diagnostics.
/// Uses the generic <see cref="FileLoggerConfiguration"/> internally.
/// </summary>
public static class WatcherLoggingConfiguration
{
    /// <summary>
    /// Creates a watcher-specific logging configuration with sensible defaults.
    /// </summary>
    public static FileLoggerConfiguration CreateWatcherConfig()
    {
        return new FileLoggerConfiguration
        {
            LogDirectory = Path.Combine(AppContext.BaseDirectory, "logs"),
            LogFileNamePattern = "vmwatcher-{date}.log",
            LogRetentionDays = 7,
            MinimumLogLevel = LogLevel.Debug,
        };
    }

    /// <summary>
    /// Gets the log directory path for the watcher.
    /// Ensures the directory exists and cleans old logs.
    /// </summary>
    public static string GetLogDirectory()
    {
        return CreateWatcherConfig().GetLogDirectory();
    }

    /// <summary>
    /// Gets the full path to the current watcher log file.
    /// </summary>
    public static string GetLogFilePath()
    {
        return CreateWatcherConfig().GetLogFilePath();
    }

    /// <summary>
    /// Configures file-based logging for the watcher using sensible defaults.
    /// </summary>
    public static void ConfigureWatcherLogging(this ILoggingBuilder builder)
    {
        CreateWatcherConfig().Configure(builder);
    }
}
