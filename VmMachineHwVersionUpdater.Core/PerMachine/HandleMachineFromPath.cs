using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EvilBaschdi.Core.Extensions;
using JetBrains.Annotations;
using VmMachineHwVersionUpdater.Core.Enums;
using VmMachineHwVersionUpdater.Core.Models;
using VmMachineHwVersionUpdater.Core.Settings;

namespace VmMachineHwVersionUpdater.Core.PerMachine
{
    /// <inheritdoc />
    public class HandleMachineFromPath : IHandleMachineFromPath
    {
        private readonly IConvertAnnotationLineBreaks _convertAnnotationLineBreaks;
        private readonly IGuestOsOutputStringMapping _guestOsOutputStringMapping;
        private readonly IPathSettings _pathSettings;
        private readonly IReadLogInformation _readLogInformation;
        private readonly IReturnValueFromVmxLine _returnValueFromVmxLine;
        private readonly IToggleToolsSyncTime _toggleToolsSyncTime;
        private readonly IToggleToolsUpgradePolicy _toggleToolsUpgradePolicy;
        private readonly IUpdateMachineVersion _updateMachineVersion;
        private readonly IVmxLineStartsWith _vmxLineStartsWith;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="guestOsOutputStringMapping"></param>
        /// <param name="pathSettings"></param>
        /// <param name="updateMachineVersion"></param>
        /// <param name="readLogInformation"></param>
        /// <param name="returnValueFromVmxLine"></param>
        /// <param name="vmxLineStartsWith"></param>
        /// <param name="convertAnnotationLineBreaks"></param>
        /// <param name="toggleToolsUpgradePolicy"></param>
        /// <param name="toggleToolsSyncTime"></param>
        public HandleMachineFromPath([NotNull] IGuestOsOutputStringMapping guestOsOutputStringMapping, [NotNull] IPathSettings pathSettings,
                                     [NotNull] IUpdateMachineVersion updateMachineVersion, [NotNull] IReadLogInformation readLogInformation,
                                     [NotNull] IReturnValueFromVmxLine returnValueFromVmxLine, [NotNull] IVmxLineStartsWith vmxLineStartsWith,
                                     [NotNull] IConvertAnnotationLineBreaks convertAnnotationLineBreaks, [NotNull] IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
                                     [NotNull] IToggleToolsSyncTime toggleToolsSyncTime)
        {
            _guestOsOutputStringMapping = guestOsOutputStringMapping ?? throw new ArgumentNullException(nameof(guestOsOutputStringMapping));
            _pathSettings = pathSettings ?? throw new ArgumentNullException(nameof(pathSettings));
            _updateMachineVersion = updateMachineVersion ?? throw new ArgumentNullException(nameof(updateMachineVersion));
            _readLogInformation = readLogInformation ?? throw new ArgumentNullException(nameof(readLogInformation));
            _returnValueFromVmxLine = returnValueFromVmxLine ?? throw new ArgumentNullException(nameof(returnValueFromVmxLine));
            _vmxLineStartsWith = vmxLineStartsWith ?? throw new ArgumentNullException(nameof(vmxLineStartsWith));
            _convertAnnotationLineBreaks = convertAnnotationLineBreaks ?? throw new ArgumentNullException(nameof(convertAnnotationLineBreaks));
            _toggleToolsUpgradePolicy = toggleToolsUpgradePolicy ?? throw new ArgumentNullException(nameof(toggleToolsUpgradePolicy));
            _toggleToolsSyncTime = toggleToolsSyncTime ?? throw new ArgumentNullException(nameof(toggleToolsSyncTime));
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
                                     => !string.IsNullOrWhiteSpace(archivePath) && !path.Equals(archivePath, StringComparison.InvariantCultureIgnoreCase) &&
                                        file.StartsWith(archivePath, StringComparison.InvariantCultureIgnoreCase)))
            {
                return null;
            }

            var readAllLines = File.ReadAllLines(file);
            var hwVersion = "0";
            var displayName = "";
            var guestOs = "";
            var guestOsDetailedData = "";
            var syncTimeWithHost = "";
            var toolsUpgradePolicy = "";
            var annotation = "";

            Parallel.ForEach(readAllLines,
                line =>
                {
                    // ReSharper disable StringLiteralTypo
                    switch (line)
                    {
                        case var _ when _vmxLineStartsWith.ValueFor(line, "virtualhw.version"):
                            hwVersion = _returnValueFromVmxLine.ValueFor(line, "virtualhw.version");
                            break;
                        case var _ when _vmxLineStartsWith.ValueFor(line, "displayname"):
                            displayName = _returnValueFromVmxLine.ValueFor(line, "displayname");
                            break;
                        case var _ when _vmxLineStartsWith.ValueFor(line, "tools.syncTime"):
                            syncTimeWithHost = _returnValueFromVmxLine.ValueFor(line, "tools.syncTime");
                            break;
                        case var _ when _vmxLineStartsWith.ValueFor(line, "tools.upgrade.policy"):
                            toolsUpgradePolicy = _returnValueFromVmxLine.ValueFor(line, "tools.upgrade.policy");
                            break;
                        case var _ when _vmxLineStartsWith.ValueFor(line, "guestos")
                                        && !_vmxLineStartsWith.ValueFor(line, "guestos.detailed.data"):
                            guestOs = _returnValueFromVmxLine.ValueFor(line, "guestos");
                            break;
                        case var _ when _vmxLineStartsWith.ValueFor(line, "guestos.detailed.data"):
                            guestOsDetailedData = _returnValueFromVmxLine.ValueFor(line, "guestOS.detailed.data");
                            break;
                        case var _ when _vmxLineStartsWith.ValueFor(line, "annotation"):
                            var rawAnnotation = _returnValueFromVmxLine.ValueFor(line, "annotation");
                            annotation = _convertAnnotationLineBreaks.ValueFor(rawAnnotation);

                            break;
                    }
                    // ReSharper restore StringLiteralTypo
                });

            var fileInfo = new FileInfo(file);
            var directoryInfo = fileInfo.Directory;
            var (logLastDate, logLastDateDiff) = _readLogInformation.ValueFor(directoryInfo?.FullName);
            var size = directoryInfo.GetDirectorySize();
            var paused = directoryInfo?.GetFiles("*.vmss").Any();
            var properFilePathCapitalization = fileInfo.GetProperFilePathCapitalization();

            var machine = new Machine(_updateMachineVersion, _toggleToolsUpgradePolicy, _toggleToolsSyncTime)
                          {
                              HwVersion = Convert.ToInt32(hwVersion),
                              DisplayName = displayName.Trim() + " " + (!string.IsNullOrWhiteSpace(annotation) ? "*" : ""),
                              GuestOs = _guestOsOutputStringMapping.ValueFor(guestOs.Trim()),
                              GuestOsDetailedData = guestOsDetailedData,
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
                                  ? MachineState.Paused
                                  : MachineState.Off,
                              Annotation = annotation
                          };
            return machine;
        }
    }
}