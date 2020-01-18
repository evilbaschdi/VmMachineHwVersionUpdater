using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.Core
{
    /// <inheritdoc />
    public class UpdateMachineVersion : IUpdateMachineVersion
    {
        /// <inheritdoc />
        public void RunFor(string vmxPath, int newVersion)
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
                if (!lineToLower.Contains("virtualhw.version"))
                {
                    continue;
                }

                var inputStreamReader = File.OpenText(vmxPath);
                var text = inputStreamReader.ReadToEnd();
                inputStreamReader.Close();
                // ReSharper disable once StringLiteralTypo
                var hwVersion = $"virtualhw.version = \"{newVersion}\"";

                text = text.Replace(line, hwVersion);

                var outputStreamWriter = File.CreateText(vmxPath);
                outputStreamWriter.Write(text);
                outputStreamWriter.Close();
            }
        }

        /// <inheritdoc />
        public void RunFor(List<Machine> machines, int newVersion)
        {
            if (machines == null)
            {
                throw new ArgumentNullException(nameof(machines));
            }

            Parallel.ForEach(machines, machine => { RunFor(machine.Path, newVersion); });
        }
    }
}