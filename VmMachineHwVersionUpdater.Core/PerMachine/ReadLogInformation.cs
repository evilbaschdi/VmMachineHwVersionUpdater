using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using EvilBaschdi.Core.Extensions;
using JetBrains.Annotations;

namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public class ReadLogInformation : IReadLogInformation
{
    /// <inheritdoc />
    public KeyValuePair<string, string> ValueFor([NotNull] string logDirectory)
    {
        if (logDirectory == null)
        {
            throw new ArgumentNullException(nameof(logDirectory));
        }

        var logLastDate = string.Empty;
        var logLastDateDiff = string.Empty;

        var log = File.Exists($@"{logDirectory}\vmware.log")
            // ReSharper disable once StringLiteralTypo
            ? $@"{logDirectory}\vmware.log"
            : null;

        if (string.IsNullOrWhiteSpace(log) || log.FileInfo().IsFileLocked())
        {
            return new(!string.IsNullOrWhiteSpace(logLastDate)
                ? logLastDate[..16]
                : string.Empty, logLastDateDiff);
        }

        try
        {
            var logLastLine = File.ReadAllLines(log).Last();
            logLastDate = logLastLine.Split('|').First().Replace("T", " ")[..23]
                                     .Replace(".", ",");
            var lastLogDateTime = DateTime.ParseExact(logLastDate, "yyyy-MM-dd HH:mm:ss,fff",
                CultureInfo.InvariantCulture);
            var logLastDiffTimeSpan = DateTime.Now - lastLogDateTime;
            logLastDateDiff =
                $"{logLastDiffTimeSpan.Days} days, {logLastDiffTimeSpan.Hours} hours and {logLastDiffTimeSpan.Minutes} minutes ago";
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return new(!string.IsNullOrWhiteSpace(logLastDate)
            ? logLastDate[..16]
            : string.Empty, logLastDateDiff);
    }
}