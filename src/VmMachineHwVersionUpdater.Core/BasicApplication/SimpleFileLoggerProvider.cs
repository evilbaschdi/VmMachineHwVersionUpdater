using Microsoft.Extensions.Logging;

namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <summary>
///     Simple file-based logging provider.
///     Can be used standalone or via FileLoggerConfiguration.
/// </summary>
internal sealed class SimpleFileLoggerProvider(string logFilePath, LogLevel minimumLogLevel = LogLevel.Debug) : ILoggerProvider
{
    private readonly string _logFilePath = logFilePath ?? throw new ArgumentNullException(nameof(logFilePath));
    private readonly LogLevel _minimumLogLevel = minimumLogLevel;
    private readonly object _lockObject = new object();

    /// <inheritdoc />
    public ILogger CreateLogger(string categoryName) => new SimpleFileLogger(_logFilePath, _minimumLogLevel, _lockObject);

    /// <inheritdoc />
    public void Dispose()
    {
        // Nothing to dispose
    }
}