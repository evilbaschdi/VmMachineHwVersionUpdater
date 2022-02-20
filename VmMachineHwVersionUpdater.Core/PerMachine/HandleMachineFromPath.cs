using System.Globalization;
using EvilBaschdi.Core.Extensions;
using JetBrains.Annotations;
using VmMachineHwVersionUpdater.Core.Enums;
using VmMachineHwVersionUpdater.Core.Models;
using VmMachineHwVersionUpdater.Core.Settings;

namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public class HandleMachineFromPath : IHandleMachineFromPath
{
    private readonly IGuestOsOutputStringMapping _guestOsOutputStringMapping;
    private readonly IParseVmxFile _parseVmxFile;
    private readonly IPathSettings _pathSettings;
    private readonly IReadLogInformation _readLogInformation;
    private readonly ISetDisplayName _setDisplayName;
    private readonly ISetMachineIsEnabledForEditing _setMachineIsEnabledForEditing;
    private readonly IToggleToolsSyncTime _toggleToolsSyncTime;
    private readonly IToggleToolsUpgradePolicy _toggleToolsUpgradePolicy;
    private readonly IUpdateMachineVersion _updateMachineVersion;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="guestOsOutputStringMapping"></param>
    /// <param name="pathSettings"></param>
    /// <param name="updateMachineVersion"></param>
    /// <param name="readLogInformation"></param>
    /// <param name="parseVmxFile"></param>
    /// <param name="toggleToolsUpgradePolicy"></param>
    /// <param name="toggleToolsSyncTime"></param>
    /// <param name="setMachineIsEnabledForEditing"></param>
    /// <param name="setDisplayName"></param>
    public HandleMachineFromPath([NotNull] IGuestOsOutputStringMapping guestOsOutputStringMapping, [NotNull] IPathSettings pathSettings,
                                 [NotNull] IUpdateMachineVersion updateMachineVersion, [NotNull] IReadLogInformation readLogInformation,
                                 [NotNull] IParseVmxFile parseVmxFile, [NotNull] IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
                                 [NotNull] IToggleToolsSyncTime toggleToolsSyncTime, [NotNull] ISetMachineIsEnabledForEditing setMachineIsEnabledForEditing,
                                 [NotNull] ISetDisplayName setDisplayName)
    {
        _guestOsOutputStringMapping = guestOsOutputStringMapping ?? throw new ArgumentNullException(nameof(guestOsOutputStringMapping));
        _pathSettings = pathSettings ?? throw new ArgumentNullException(nameof(pathSettings));
        _updateMachineVersion = updateMachineVersion ?? throw new ArgumentNullException(nameof(updateMachineVersion));
        _readLogInformation = readLogInformation ?? throw new ArgumentNullException(nameof(readLogInformation));
        _parseVmxFile = parseVmxFile ?? throw new ArgumentNullException(nameof(parseVmxFile));
        _toggleToolsUpgradePolicy = toggleToolsUpgradePolicy ?? throw new ArgumentNullException(nameof(toggleToolsUpgradePolicy));
        _toggleToolsSyncTime = toggleToolsSyncTime ?? throw new ArgumentNullException(nameof(toggleToolsSyncTime));
        _setMachineIsEnabledForEditing = setMachineIsEnabledForEditing ?? throw new ArgumentNullException(nameof(setMachineIsEnabledForEditing));
        _setDisplayName = setDisplayName ?? throw new ArgumentNullException(nameof(setDisplayName));
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

        var rawMachine = _parseVmxFile.ValueFor(file);

        var fileInfo = new FileInfo(file);
        var directoryInfo = fileInfo.Directory;
        var (logLastDate, logLastDateDiff) = _readLogInformation.ValueFor(directoryInfo?.FullName);
        var size = directoryInfo.GetDirectorySize();
        var paused = directoryInfo?.GetFiles("*.vmss").Any();
        var properFilePathCapitalization = fileInfo.GetProperFilePathCapitalization();

        var machine = new Machine(_updateMachineVersion, _toggleToolsUpgradePolicy, _toggleToolsSyncTime)
                      {
                          HwVersion = rawMachine.HwVersion,
                          GuestOs = _guestOsOutputStringMapping.ValueFor(rawMachine.GuestOs.Trim()),
                          GuestOsDetailedData = rawMachine.DetailedData,
                          Path = properFilePathCapitalization,
                          Directory = path,
                          ShortPath = properFilePathCapitalization.Replace(path, "",
                              StringComparison.CurrentCultureIgnoreCase),
                          DirectorySizeGb = Math.Round(size.KiBiBytesToGiBiBytes(), 2),
                          DirectorySize = size.ToFileSize(2, CultureInfo.GetCultureInfo(1033)),
                          LogLastDate = logLastDate,
                          LogLastDateDiff = logLastDateDiff,
                          AutoUpdateTools = !string.IsNullOrWhiteSpace(rawMachine.ToolsUpgradePolicy) &&
                                            rawMachine.ToolsUpgradePolicy.Equals("upgradeAtPowerCycle"),
                          SyncTimeWithHost = !string.IsNullOrWhiteSpace(rawMachine.SyncTimeWithHost) &&
                                             bool.Parse(rawMachine.SyncTimeWithHost),
                          MachineState = paused.HasValue && paused.Value
                              ? MachineState.Paused
                              : MachineState.Off,
                          Annotation = rawMachine.Annotation,
                          EncryptionData = rawMachine.EncryptionData,
                          EncryptionEncryptedKey = rawMachine.EncryptionEncryptedKey,
                          EncryptionKeySafe = rawMachine.EncryptionKeySafe,
                          ManagedVmAutoAddVTpm = rawMachine.ManagedVmAutoAddVTpm
                      };

        _setMachineIsEnabledForEditing.RunFor(machine);
        _setDisplayName.RunFor(rawMachine, machine);
        return machine;
    }
}