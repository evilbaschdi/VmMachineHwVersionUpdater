using System.Globalization;
using EvilBaschdi.Core.Extensions;
using VmMachineHwVersionUpdater.Core.Enums;

namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public class HandleMachineFromPath(
    [NotNull] IParseVmxFile parseVmxFile,
    [NotNull] ISetDisplayName setDisplayName,
    [NotNull] IToggleToolsSyncTime toggleToolsSyncTime,
    [NotNull] IUpdateMachineVersion updateMachineVersion,
    [NotNull] IUpdateMachineMemSize updateMachineMemSize,
    [NotNull] IGuestOsOutputStringMapping guestOsOutputStringMapping,
    [NotNull] IPathSettings pathSettings,
    [NotNull] IReadLogInformation readLogInformation,
    [NotNull] ISetMachineIsEnabledForEditing setMachineIsEnabledForEditing,
    [NotNull] IToggleToolsUpgradePolicy toggleToolsUpgradePolicy) : IHandleMachineFromPath
{
    private readonly IGuestOsOutputStringMapping _guestOsOutputStringMapping = guestOsOutputStringMapping ?? throw new ArgumentNullException(nameof(guestOsOutputStringMapping));
    private readonly IParseVmxFile _parseVmxFile = parseVmxFile ?? throw new ArgumentNullException(nameof(parseVmxFile));
    private readonly IPathSettings _pathSettings = pathSettings ?? throw new ArgumentNullException(nameof(pathSettings));
    private readonly IReadLogInformation _readLogInformation = readLogInformation ?? throw new ArgumentNullException(nameof(readLogInformation));
    private readonly ISetDisplayName _setDisplayName = setDisplayName ?? throw new ArgumentNullException(nameof(setDisplayName));

    private readonly ISetMachineIsEnabledForEditing _setMachineIsEnabledForEditing =
        setMachineIsEnabledForEditing ?? throw new ArgumentNullException(nameof(setMachineIsEnabledForEditing));

    private readonly IToggleToolsSyncTime _toggleToolsSyncTime = toggleToolsSyncTime ?? throw new ArgumentNullException(nameof(toggleToolsSyncTime));
    private readonly IToggleToolsUpgradePolicy _toggleToolsUpgradePolicy = toggleToolsUpgradePolicy ?? throw new ArgumentNullException(nameof(toggleToolsUpgradePolicy));
    private readonly IUpdateMachineVersion _updateMachineVersion = updateMachineVersion ?? throw new ArgumentNullException(nameof(updateMachineVersion));
    private readonly IUpdateMachineMemSize _updateMachineMemSize = updateMachineMemSize ?? throw new ArgumentNullException(nameof(updateMachineMemSize));

    /// <inheritdoc />
    public Machine ValueFor(MachinePath machinePath)
    {
        ArgumentNullException.ThrowIfNull(machinePath);

        var archivePaths = _pathSettings.ArchivePath;
        var machinePoolPath = machinePath.MachinePoolPath;
        var machineFilePath = machinePath.MachineFilePath;

        if (machineFilePath.FileInfo().IsFileLocked() ||
            //has to be done to not handle the archived machines with the non-archived
            archivePaths.Any(archivePath
                                 => !string.IsNullOrWhiteSpace(archivePath) && !machinePoolPath.Equals(archivePath, StringComparison.InvariantCultureIgnoreCase) &&
                                    machineFilePath.StartsWith(archivePath, StringComparison.InvariantCultureIgnoreCase)))
        {
            return null;
        }

        var rawMachine = _parseVmxFile.ValueFor(machineFilePath);

        var fileInfo = new FileInfo(machineFilePath);
        var directoryInfo = fileInfo.Directory;
        var (logLastDate, logLastDateDiff) = _readLogInformation.ValueFor(directoryInfo?.FullName);
        var size = directoryInfo.GetDirectorySize();
        var paused = directoryInfo?.GetFiles("*.vmss").Any();
        var properFilePathCapitalization = fileInfo.GetProperFilePathCapitalization();

        var machine = new Machine(_toggleToolsSyncTime, _toggleToolsUpgradePolicy, _updateMachineVersion, _updateMachineMemSize)
                      {
                          DisplayName = rawMachine.DisplayName,
                          HwVersion = rawMachine.HwVersion,
                          MemSize = rawMachine.MemSize / 1024,
                          GuestOs = _guestOsOutputStringMapping.ValueFor(rawMachine.GuestOs.Trim()),
                          GuestOsDetailedData = rawMachine.DetailedData,
                          Path = properFilePathCapitalization,
                          Directory = machinePoolPath,
                          ShortPath = properFilePathCapitalization.Replace(machinePoolPath, "",
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