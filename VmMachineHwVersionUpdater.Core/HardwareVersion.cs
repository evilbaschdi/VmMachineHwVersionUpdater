﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EvilBaschdi.Core.Extensions;
using EvilBaschdi.Core.Internal;
using EvilBaschdi.Core.Model;
using MahApps.Metro.IconPacks;
using VmMachineHwVersionUpdater.Core.Models;
using VmMachineHwVersionUpdater.Core.Settings;

namespace VmMachineHwVersionUpdater.Core
{
    /// <inheritdoc />
    public class HardwareVersion : IHardwareVersion
    {
        private readonly IGuestOsOutputStringMapping _guestOsOutputStringMapping;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="guestOsOutputStringMapping"></param>
        public HardwareVersion(IGuestOsOutputStringMapping guestOsOutputStringMapping)
        {
            _guestOsOutputStringMapping = guestOsOutputStringMapping ??
                                          throw new ArgumentNullException(nameof(guestOsOutputStringMapping));
        }

        /// <inheritdoc />
        /// <param name="vmxPath"></param>
        /// <param name="newVersion"></param>
        public void Update(string vmxPath, int newVersion)
        {
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
        /// <param name="vmxPath"></param>
        /// <param name="syncTimeWithHost"></param>
        public void EnableSyncTimeWithHost(string vmxPath, bool syncTimeWithHost)
        {
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

            EnableToolsAutoUpdate(vmxPath, true);
        }

        /// <inheritdoc />
        /// <param name="vmxPath"></param>
        /// <param name="toolsAutoUpdate"></param>
        public void EnableToolsAutoUpdate(string vmxPath, bool toolsAutoUpdate)
        {
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
            //SetApplyHostDisplayScalingToGuest(vmxPath, false);
        }

        /// <inheritdoc />
        /// <param name="machinePaths"></param>
        /// <param name="archivePaths"></param>
        /// <returns></returns>
        public IEnumerable<Machine> ReadFromPath(List<string> machinePaths, List<string> archivePaths)
        {
            var multiThreading = new MultiThreading();
            var fileListFromPath = new FileListFromPath(multiThreading);
            var machineList = new ConcurrentBag<Machine>();
            var filterExtensionsToEqual = new List<string>
                                          {
                                              "vmx"
                                          };


            var fileListFromPathFilter = new FileListFromPathFilter
                                         {
                                             FilterExtensionsToEqual = filterExtensionsToEqual
                                         };
            var innerPaths = machinePaths;
            innerPaths.AddRange(archivePaths);

            foreach (var path in machinePaths)
            {
                if (!Directory.Exists(path))
                {
                    continue;
                }

                var fileList = fileListFromPath.ValueFor(path, fileListFromPathFilter).Distinct().ToList();

                Parallel.ForEach(fileList,
                    file =>
                    {
                        if (file.FileInfo().IsFileLocked())
                        {
                            return;
                        }

                        foreach (var archivePath in archivePaths)
                        {
                            if (!path.Equals(archivePath, StringComparison.CurrentCultureIgnoreCase) &&
                                file.StartsWith(archivePath, StringComparison.CurrentCultureIgnoreCase))
                            {
                                return;
                            }
                        }

                        var readAllLines = File.ReadAllLines(file);
                        var hwVersion = "";
                        var displayName = "";
                        var guestOs = "";
                        var syncTimeWithHost = "";
                        var toolsUpgradePolicy = "";

                        Parallel.ForEach(readAllLines,
                            line =>
                            {
                                // ReSharper disable once StringLiteralTypo
                                if (line.StartsWith("virtualhw.version", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    hwVersion = line.Replace('"', ' ').Trim();
                                    hwVersion = Regex.Replace(hwVersion, "virtualhw.version = ", "",
                                        RegexOptions.IgnoreCase).Trim();
                                }

                                if (line.StartsWith("displayname", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    displayName = line.Replace('"', ' ').Trim();
                                    displayName = Regex.Replace(displayName, "displayname = ", "",
                                        RegexOptions.IgnoreCase).Trim();
                                }

                                // ReSharper disable once StringLiteralTypo
                                if (line.StartsWith("guestos", StringComparison.CurrentCultureIgnoreCase) &&
                                    !line.StartsWith("guestos.detailed.data", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    guestOs = line.Replace('"', ' ').Trim();
                                    guestOs = Regex.Replace(guestOs, "guestos = ", "", RegexOptions.IgnoreCase).Trim();
                                }

                                if (line.StartsWith("tools.syncTime", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    syncTimeWithHost = line.Replace('"', ' ').Trim();
                                    syncTimeWithHost = Regex.Replace(syncTimeWithHost, "tools.syncTime = ", "",
                                        RegexOptions.IgnoreCase).Trim();
                                }

                                // ReSharper disable once InvertIf
                                if (line.StartsWith("tools.upgrade.policy", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    toolsUpgradePolicy = line.Replace('"', ' ').Trim();
                                    toolsUpgradePolicy = Regex
                                                         .Replace(toolsUpgradePolicy, "tools.upgrade.policy = ", "",
                                                             RegexOptions.IgnoreCase)
                                                         .Trim();
                                }
                            });

                        var fileInfo = new FileInfo(file);
                        var directoryInfo = fileInfo.Directory;
                        var log = File.Exists($@"{directoryInfo?.FullName}\vmware.log")
                            // ReSharper disable once StringLiteralTypo
                            ? $@"{directoryInfo?.FullName}\vmware.log"
                            : null;
                        var logLastDate = string.Empty;
                        var logLastDateDiff = string.Empty;

                        if (!string.IsNullOrWhiteSpace(log) && !log.FileInfo().IsFileLocked())
                        {
                            try
                            {
                                var logLastLine = File.ReadAllLines(log).Last();
                                logLastDate = logLastLine.Split('|').First().Replace("T", " ").Substring(0, 23)
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
                        }

                        var size = directoryInfo.GetDirectorySize();
                        var paused = directoryInfo?.GetFiles("*.vmss").Any();
                        var properFilePathCapitalization = fileInfo.GetProperFilePathCapitalization();
                        var machine = new Machine(this)
                                      {
                                          Id = Guid.NewGuid().ToString(),
                                          HwVersion = Convert.ToInt32(hwVersion),
                                          DisplayName = displayName.Trim(),
                                          GuestOs = _guestOsOutputStringMapping.ValueFor(guestOs.Trim()),
                                          Path = properFilePathCapitalization,
                                          Directory = path,
                                          ShortPath =
                                              properFilePathCapitalization.Replace(path, "",
                                                  StringComparison.CurrentCultureIgnoreCase),
                                          DirectorySizeGb = Math.Round(size.KiBiBytesToGiBiBytes(), 2),
                                          DirectorySize = size.ToFileSize(2, CultureInfo.GetCultureInfo(1033)),
                                          LogLastDate = !string.IsNullOrWhiteSpace(logLastDate)
                                              ? logLastDate.Substring(0, 16)
                                              : string.Empty,
                                          LogLastDateDiff = logLastDateDiff,
                                          AutoUpdateTools =
                                              !string.IsNullOrWhiteSpace(toolsUpgradePolicy) &&
                                              toolsUpgradePolicy.Equals("upgradeAtPowerCycle"),
                                          SyncTimeWithHost = !string.IsNullOrWhiteSpace(syncTimeWithHost) &&
                                                             bool.Parse(syncTimeWithHost),
                                          MachineState = paused.HasValue && paused.Value
                                              ? PackIconMaterialKind.Pause
                                              : PackIconMaterialKind.Power
                                      };
                        machineList.Add(machine);
                    });
            }

            return machineList;
        }

        /// <summary>
        /// </summary>
        /// <param name="vmxPath"></param>
        /// <param name="isEnabled"></param>
        // ReSharper disable once UnusedMember.Global
        public void SetApplyHostDisplayScalingToGuest(string vmxPath, bool isEnabled)
        {
            var readAllLines = File.ReadAllLines(vmxPath);
            var lineContained = false;
            var inputStreamReader = File.OpenText(vmxPath);
            var text = inputStreamReader.ReadToEnd();
            inputStreamReader.Close();

            var newLine = isEnabled
                ? "gui.applyHostDisplayScalingToGuest = \"TRUE\""
                : "gui.applyHostDisplayScalingToGuest = \"FALSE\"";

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