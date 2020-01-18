using System;
using System.IO;

namespace VmMachineHwVersionUpdater.Core
{
    /// <inheritdoc />
    public class EnableToolsAutoUpdate : IEnableToolsAutoUpdate
    {
        /// <inheritdoc />
        public void RunFor(string vmxPath, bool toolsAutoUpdate)
        {
            if (string.IsNullOrWhiteSpace(vmxPath))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(vmxPath));
            }

            var readAllLines = File.ReadAllLines(vmxPath);
            var lineContained = false;
            var inputStreamReader = File.OpenText(vmxPath);
            var text = inputStreamReader.ReadToEnd();
            inputStreamReader.Close();

            var newLine = toolsAutoUpdate
                ? "tools.upgrade.policy = \"upgradeAtPowerCycle\""
                : "tools.upgrade.policy = \"useGlobal\"";

            foreach (var line in readAllLines)
            {
                var lineToLower = line.ToLower();
                //tools.upgrade.policy = "upgradeAtPowerCycle"
                if (!lineToLower.Contains("tools.upgrade.policy"))
                {
                    continue;
                }

                lineContained = true;

                text = text.Replace(line, newLine);
            }

            if (!lineContained)
            {
                text = $"{text}{Environment.NewLine}{newLine}";
            }

            var outputStreamWriter = File.CreateText(vmxPath);
            outputStreamWriter.Write(text);
            outputStreamWriter.Close();
        }
    }
}