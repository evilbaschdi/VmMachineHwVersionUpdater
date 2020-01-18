using System;
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
    public class MachinesFromPath : IMachinesFromPath
    {
        private readonly IGuestOsOutputStringMapping _guestOsOutputStringMapping;
        private readonly IPathSettings _pathSettings;
        private readonly IUpdateMachineVersion _updateMachineVersion;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="guestOsOutputStringMapping"></param>
        /// <param name="pathSettings"></param>
        /// <param name="updateMachineVersion"></param>
        public MachinesFromPath(IGuestOsOutputStringMapping guestOsOutputStringMapping, IPathSettings pathSettings, IUpdateMachineVersion updateMachineVersion)
        {
            _guestOsOutputStringMapping = guestOsOutputStringMapping ??
                                          throw new ArgumentNullException(nameof(guestOsOutputStringMapping));
            _pathSettings = pathSettings ?? throw new ArgumentNullException(nameof(pathSettings));
            _updateMachineVersion = updateMachineVersion ?? throw new ArgumentNullException(nameof(updateMachineVersion));
        }


        /// <inheritdoc />
        public List<Machine> Value
        {
            get
            {
                var machinePaths = _pathSettings.VmPool;
                var archivePaths = _pathSettings.ArchivePath;
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
                            var machine = new Machine(_updateMachineVersion)
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

                return machineList.ToList();
            }
        }
    }
}