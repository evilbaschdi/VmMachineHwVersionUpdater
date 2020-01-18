using System;
using System.IO;

namespace VmMachineHwVersionUpdater.Core
{
    /// <inheritdoc />
    public class EnableSyncTimeWithHost : IEnableSyncTimeWithHost
    {
        /// <inheritdoc />
        public void RunFor(string vmxPath, bool syncTimeWithHost)
        {
            if (string.IsNullOrWhiteSpace(vmxPath))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(vmxPath));
            }

            var readAllLines = File.ReadAllLines(vmxPath);

            foreach (var line in readAllLines)
            {
                var lineToLower = line.ToLower();
                // ReSharper disable once StringLiteralTypo
                if (!lineToLower.Contains("tools.synctime"))
                {
                    continue;
                }

                var inputStreamReader = File.OpenText(vmxPath);
                var text = inputStreamReader.ReadToEnd();
                inputStreamReader.Close();
                var syncTimeWithHostString = syncTimeWithHost ? "TRUE" : "FALSE";
                var newLine = $"tools.syncTime = \"{syncTimeWithHostString}\"";

                text = text.Replace(line, newLine);

                var outputStreamWriter = File.CreateText(vmxPath);
                outputStreamWriter.Write(text);
                outputStreamWriter.Close();
            }
        }
    }
}