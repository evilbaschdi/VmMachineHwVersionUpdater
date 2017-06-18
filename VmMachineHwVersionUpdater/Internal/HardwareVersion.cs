using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EvilBaschdi.Core.DirectoryExtensions;
using EvilBaschdi.Core.Threading;
using VmMachineHwVersionUpdater.Core;
using VmMachineHwVersionUpdater.Model;

namespace VmMachineHwVersionUpdater.Internal
{
    /// <summary>
    /// </summary>
    public class HardwareVersion : IHardwareVersion
    {
        private readonly IGuestOsOutputStringMapping _guestOsOutputStringMapping;

        /// <summary>Initialisiert eine neue Instanz der <see cref="T:System.Object" />-Klasse.</summary>
        /// <exception cref="ArgumentNullException"><paramref name="guestOsOutputStringMapping" /> is <see langword="null" />.</exception>
        public HardwareVersion(IGuestOsOutputStringMapping guestOsOutputStringMapping)
        {
            if (guestOsOutputStringMapping == null)
            {
                throw new ArgumentNullException(nameof(guestOsOutputStringMapping));
            }
            _guestOsOutputStringMapping = guestOsOutputStringMapping;
        }

        /// <summary>
        /// </summary>
        /// <param name="vmxPath"></param>
        /// <param name="newVersion"></param>
        public void Update(string vmxPath, int newVersion)
        {
            var readAllLines = File.ReadAllLines(vmxPath);

            foreach (var line in readAllLines)
            {
                var lineToLower = line.ToLower();

                if (!lineToLower.Contains("virtualhw.version"))
                {
                    continue;
                }
                var inputStreamReader = File.OpenText(vmxPath);
                var text = inputStreamReader.ReadToEnd();
                inputStreamReader.Close();

                var hwversion = $"virtualhw.version = \"{newVersion}\"";

                text = text.Replace(line, hwversion);

                var outputStreamWriter = File.CreateText(vmxPath);
                outputStreamWriter.Write(text);
                outputStreamWriter.Close();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="vmxPath"></param>
        /// <param name="syncTimeWithHost"></param>
        public void EnableSyncTimeWithHost(string vmxPath, bool syncTimeWithHost)
        {
            var readAllLines = File.ReadAllLines(vmxPath);

            foreach (var line in readAllLines)
            {
                var lineToLower = line.ToLower();

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

            EnableToolsAutoUpdate(vmxPath, true);
        }

        /// <summary>
        /// </summary>
        /// <param name="vmxPath"></param>
        /// <param name="toolsAutoUpdate"></param>
        public void EnableToolsAutoUpdate(string vmxPath, bool toolsAutoUpdate)
        {
            var readAllLines = File.ReadAllLines(vmxPath);
            var lineContained = false;
            var inputStreamReader = File.OpenText(vmxPath);
            var text = inputStreamReader.ReadToEnd();
            inputStreamReader.Close();

            var newLine = toolsAutoUpdate ? "tools.upgrade.policy = \"upgradeAtPowerCycle\"" : "tools.upgrade.policy = \"useGlobal\"";

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
            //SetApplyHostDisplayScalingToGuest(vmxPath, false);
        }

        /// <summary>
        /// </summary>
        /// <param name="machinePath"></param>
        /// <returns></returns>
        public IEnumerable<Machine> ReadFromPath(string machinePath)
        {
            var multiThreadingHelper = new MultiThreadingHelper();
            var filePath = new FilePath(multiThreadingHelper);
            var machineList = new ConcurrentBag<Machine>();
            var includeExtensionList = new List<string>
                                       {
                                           "vmx"
                                       };


            var machinePaths = machinePath.SplitToList(";");

            Parallel.ForEach(machinePaths, path =>
                                           {
                                               if (!Directory.Exists(path))
                                               {
                                                   return;
                                               }
                                               var fileList = filePath.GetFileList(path, includeExtensionList).Distinct().ToList();

                                               Parallel.ForEach(fileList,
                                                   file =>
                                                   {
                                                       var readAllLines = File.ReadAllLines(file);
                                                       var hwVersion = "";
                                                       var displayName = "";
                                                       var guestOs = "";
                                                       var syncTimeWithHost = "";
                                                       var toolsUpgradePolicy = "";

                                                       Parallel.ForEach(readAllLines,
                                                           line =>
                                                           {
                                                               if (line.StartsWith("virtualhw.version", StringComparison.CurrentCultureIgnoreCase))
                                                               {
                                                                   hwVersion = line.Replace('"', ' ').Trim();
                                                                   hwVersion = Regex.Replace(hwVersion, "virtualhw.version = ", "", RegexOptions.IgnoreCase).Trim();
                                                               }
                                                               if (line.StartsWith("displayname", StringComparison.CurrentCultureIgnoreCase))
                                                               {
                                                                   displayName = line.Replace('"', ' ').Trim();
                                                                   displayName = Regex.Replace(displayName, "displayname = ", "", RegexOptions.IgnoreCase).Trim();
                                                               }
                                                               if (line.StartsWith("guestos", StringComparison.CurrentCultureIgnoreCase))
                                                               {
                                                                   guestOs = line.Replace('"', ' ').Trim();
                                                                   guestOs = Regex.Replace(guestOs, "guestos = ", "", RegexOptions.IgnoreCase).Trim();
                                                               }

                                                               if (line.StartsWith("tools.syncTime", StringComparison.CurrentCultureIgnoreCase))
                                                               {
                                                                   syncTimeWithHost = line.Replace('"', ' ').Trim();
                                                                   syncTimeWithHost = Regex.Replace(syncTimeWithHost, "tools.syncTime = ", "", RegexOptions.IgnoreCase).Trim();
                                                               }

                                                               if (line.StartsWith("tools.upgrade.policy", StringComparison.CurrentCultureIgnoreCase))
                                                               {
                                                                   toolsUpgradePolicy = line.Replace('"', ' ').Trim();
                                                                   toolsUpgradePolicy = Regex
                                                                       .Replace(toolsUpgradePolicy, "tools.upgrade.policy = ", "", RegexOptions.IgnoreCase)
                                                                       .Trim();
                                                               }
                                                           });

                                                       var fileInfo = new FileInfo(file);
                                                       var directoryInfo = fileInfo.Directory;
                                                       var log = File.Exists($@"{directoryInfo?.FullName}\vmware.log") ? $@"{directoryInfo?.FullName}\vmware.log" : null;
                                                       var logLastDate = string.Empty;
                                                       var logLastDateDiff = string.Empty;

                                                       if (!string.IsNullOrWhiteSpace(log) && !log.IsFileLocked())
                                                       {
                                                           var logLastLine = File.ReadAllLines(log).Last();
                                                           logLastDate = logLastLine.Split('|').First().Replace("T", " ").Substring(0, 23).Replace(".", ",");
                                                           var lastLogDateTime = DateTime.ParseExact(logLastDate, "yyyy-MM-dd HH:mm:ss,fff", CultureInfo.InvariantCulture);
                                                           var logLastDiffTimeSpan = DateTime.Now - lastLogDateTime;
                                                           logLastDateDiff =
                                                               $"{logLastDiffTimeSpan.Days} days, {logLastDiffTimeSpan.Hours} hours and {logLastDiffTimeSpan.Minutes} minutes ago";
                                                       }


                                                       var size = directoryInfo.GetDirectorySize();
                                                       var properFilePathCapitalization = fileInfo.GetProperFilePathCapitalization();
                                                       var machine = new Machine
                                                                     {
                                                                         Id = Guid.NewGuid().ToString(),
                                                                         HwVersion = Convert.ToInt32(hwVersion),
                                                                         DisplayName = displayName.Trim(),
                                                                         GuestOs = _guestOsOutputStringMapping.ValueFor(guestOs.Trim()),
                                                                         Path = properFilePathCapitalization,
                                                                         Directory = path,
                                                                         ShortPath =
                                                                             properFilePathCapitalization.Replace(path, "", StringComparison.CurrentCultureIgnoreCase),
                                                                         DirectorySizeGb = Math.Round(size / (1024 * 1024 * 1024), 2),
                                                                         DirectorySize = $"MB: {Math.Round(size / (1024 * 1024), 2)} | KB: {Math.Round(size / 1024, 2)}",
                                                                         LogLastDate = !string.IsNullOrWhiteSpace(logLastDate) ? logLastDate.Substring(0, 16) : string.Empty,
                                                                         LogLastDateDiff = logLastDateDiff,
                                                                         AutoUpdateTools =
                                                                             !string.IsNullOrWhiteSpace(toolsUpgradePolicy) && toolsUpgradePolicy.Equals("upgradeAtPowerCycle"),
                                                                         SyncTimeWithHost = !string.IsNullOrWhiteSpace(syncTimeWithHost) && bool.Parse(syncTimeWithHost)
                                                                     };
                                                       machineList.Add(machine);
                                                   });
                                           });
            return machineList;
        }

        /// <summary>
        /// </summary>
        /// <param name="vmxPath"></param>
        /// <param name="isEnabled"></param>
        public void SetApplyHostDisplayScalingToGuest(string vmxPath, bool isEnabled)
        {
            var readAllLines = File.ReadAllLines(vmxPath);
            var lineContained = false;
            var inputStreamReader = File.OpenText(vmxPath);
            var text = inputStreamReader.ReadToEnd();
            inputStreamReader.Close();

            var newLine = isEnabled ? "gui.applyHostDisplayScalingToGuest = \"TRUE\"" : "gui.applyHostDisplayScalingToGuest = \"FALSE\"";

            foreach (var line in readAllLines)
            {
                var lineToLower = line.ToLower();

                //gui.applyHostDisplayScalingToGuest
                if (!lineToLower.Contains("gui.applyHostDisplayScalingToGuest"))
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