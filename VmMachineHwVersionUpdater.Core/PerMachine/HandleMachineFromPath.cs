using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EvilBaschdi.Core.Extensions;
using JetBrains.Annotations;
using MahApps.Metro.IconPacks;
using VmMachineHwVersionUpdater.Core.Models;
using VmMachineHwVersionUpdater.Core.Settings;

namespace VmMachineHwVersionUpdater.Core.PerMachine
{
    /// <inheritdoc />
    public class HandleMachineFromPath : IHandleMachineFromPath
    {
        private readonly IGuestOsOutputStringMapping _guestOsOutputStringMapping;
        private readonly IPathSettings _pathSettings;
        private readonly IReadLogInformation _readLogInformation;
        private readonly IUpdateMachineVersion _updateMachineVersion;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="guestOsOutputStringMapping"></param>
        /// <param name="pathSettings"></param>
        /// <param name="updateMachineVersion"></param>
        /// <param name="readLogInformation"></param>
        public HandleMachineFromPath([NotNull] IGuestOsOutputStringMapping guestOsOutputStringMapping, [NotNull] IPathSettings pathSettings,
                                     [NotNull] IUpdateMachineVersion updateMachineVersion, [NotNull] IReadLogInformation readLogInformation)
        {
            _guestOsOutputStringMapping = guestOsOutputStringMapping ?? throw new ArgumentNullException(nameof(guestOsOutputStringMapping));
            _pathSettings = pathSettings ?? throw new ArgumentNullException(nameof(pathSettings));
            _updateMachineVersion = updateMachineVersion ?? throw new ArgumentNullException(nameof(updateMachineVersion));
            _readLogInformation = readLogInformation ?? throw new ArgumentNullException(nameof(readLogInformation));
        }

        /// <inheritdoc />
        public Machine ValueFor([NotNull] string path, [NotNull] string file)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var archivePaths = _pathSettings.ArchivePath;

            if (file.FileInfo().IsFileLocked() ||
                //has to be done to not handle the archived machines with the non-archived
                archivePaths.Any(archivePath
                                     => !path.Equals(archivePath, StringComparison.CurrentCultureIgnoreCase) &&
                                        file.StartsWith(archivePath, StringComparison.CurrentCultureIgnoreCase)))
            {
                return null;
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
            var (logLastDate, logLastDateDiff) = _readLogInformation.ValueFor(directoryInfo?.FullName);
            var size = directoryInfo.GetDirectorySize();
            var paused = directoryInfo?.GetFiles("*.vmss").Any();
            var properFilePathCapitalization = fileInfo.GetProperFilePathCapitalization();
            var machine = new Machine(_updateMachineVersion)
                          {
                              HwVersion = Convert.ToInt32(hwVersion),
                              DisplayName = displayName.Trim(),
                              GuestOs = _guestOsOutputStringMapping.ValueFor(guestOs.Trim()),
                              Path = properFilePathCapitalization,
                              Directory = path,
                              ShortPath = properFilePathCapitalization.Replace(path, "",
                                  StringComparison.CurrentCultureIgnoreCase),
                              DirectorySizeGb = Math.Round(size.KiBiBytesToGiBiBytes(), 2),
                              DirectorySize = size.ToFileSize(2, CultureInfo.GetCultureInfo(1033)),
                              LogLastDate = logLastDate,
                              LogLastDateDiff = logLastDateDiff,
                              AutoUpdateTools = !string.IsNullOrWhiteSpace(toolsUpgradePolicy) &&
                                                toolsUpgradePolicy.Equals("upgradeAtPowerCycle"),
                              SyncTimeWithHost = !string.IsNullOrWhiteSpace(syncTimeWithHost) &&
                                                 bool.Parse(syncTimeWithHost),
                              MachineState = paused.HasValue && paused.Value
                                  ? PackIconMaterialKind.Pause
                                  : PackIconMaterialKind.Power
                          };
            return machine;
        }
    }
}