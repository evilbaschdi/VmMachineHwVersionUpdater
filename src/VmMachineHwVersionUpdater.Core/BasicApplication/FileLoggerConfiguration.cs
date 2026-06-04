using Microsoft.Extensions.Logging;

namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <summary>
///     Generic configuration for file-based logging that can be reused across different applications and libraries.
/// </summary>
public class FileLoggerConfiguration
{
    /// <summary>
    ///     Gets or sets the base directory where logs will be stored.
    ///     Defaults to {AppContext.BaseDirectory}/logs
    /// </summary>
    public string LogDirectory { get; set; } = Path.Combine(AppContext.BaseDirectory, "logs");

    /// <summary>
    ///     Gets or sets the log file name pattern.
    ///     Use {date} as a placeholder for the current date (yyyy-MM-dd format).
    ///     Example: "myapp-{date}.log" becomes "myapp-2026-06-04.log"
    /// </summary>
    public string LogFileNamePattern { get; set; } = "app-{date}.log";

    /// <summary>
    ///     Gets or sets the number of days to retain log files.
    ///     Log files older than this will be automatically deleted.
    /// </summary>
    public int LogRetentionDays { get; set; } = 7;

    /// <summary>
    ///     Gets or sets the minimum log level.
    /// </summary>
    public LogLevel MinimumLogLevel { get; set; } = LogLevel.Debug;

    /// <summary>
    ///     Gets the log directory path, ensuring it exists.
    ///     Cleans up old log files based on retention policy.
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
    ///     Gets the full path to the current log file.
    /// </summary>
    public string GetLogFilePath()
    {
        var logDir = GetLogDirectory();
        var logFileName = LogFileNamePattern.Replace("{date}", DateTime.Now.ToString("yyyy-MM-dd"));
        return Path.Combine(logDir, logFileName);
    }

    /// <summary>
    ///     Cleans up log files older than the retention period.
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
    ///     Configures file-based logging using this configuration.
    /// </summary>
    public void Configure(ILoggingBuilder builder)
    {
        builder.ClearProviders();
        builder.SetMinimumLevel(MinimumLogLevel);
        builder.AddProvider(new SimpleFileLoggerProvider(GetLogFilePath(), MinimumLogLevel));
    }
}