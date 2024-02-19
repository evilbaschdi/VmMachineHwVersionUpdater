using System.Globalization;
using EvilBaschdi.Core.Extensions;

namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public class ReadLogInformation : IReadLogInformation
{
    /// <inheritdoc />
    public KeyValuePair<string, string> ValueFor([NotNull] string logDirectory)
    {
        ArgumentNullException.ThrowIfNull(logDirectory);

        var lastLogDateLocal = string.Empty;
        var logLastDateDiff = string.Empty;

        var log = File.Exists($@"{logDirectory}\vmware.log")
            // ReSharper disable once StringLiteralTypo
            ? $@"{logDirectory}\vmware.log"
            : null;

        if (string.IsNullOrWhiteSpace(log) || log.FileInfo().IsFileLocked())
        {
            return new(lastLogDateLocal, logLastDateDiff);
        }

        try
        {
            var logLastLine = File.ReadAllLines(log).Last();
            var logLastDate = logLastLine.Split('|').First().Replace("T", " ")[..23].Replace(".", ",");
            var lastLogDateTimeUtc = DateTime.ParseExact(logLastDate, "yyyy-MM-dd HH:mm:ss,fff", CultureInfo.InvariantCulture);
            var lastLogDateTimeLocal = lastLogDateTimeUtc.ToLocalTime();

            var logLastDiffTimeSpan = DateTime.Now - lastLogDateTimeLocal;

            lastLogDateLocal = lastLogDateTimeLocal.ToString("yyyy-MM-dd HH:mm:ss");
            logLastDateDiff = $"{logLastDiffTimeSpan.Days} days, {logLastDiffTimeSpan.Hours} hours and {logLastDiffTimeSpan.Minutes} minutes ago";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return new(lastLogDateLocal, logLastDateDiff);
    }
}