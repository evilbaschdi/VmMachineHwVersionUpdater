using System.Globalization;
using EvilBaschdi.Core.Extensions;

namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public class HandleMachineFromPath(
    [NotNull] IMachineParserStrategy machineParserStrategy,
    [NotNull] ISetDisplayName setDisplayName,
    [NotNull] IToggleToolsSyncTime toggleToolsSyncTime,
    [NotNull] IUpdateMachineVersion updateMachineVersion,
    [NotNull] IUpdateMachineMemSize updateMachineMemSize,
    [NotNull] IGuestOsOutputStringMapping guestOsOutputStringMapping,
    [NotNull] IPathSettings pathSettings,
    [NotNull] IReadLogInformation readLogInformation,
    [NotNull] ISetMachineIsEnabledForEditing setMachineIsEnabledForEditing,
    [NotNull] IToggleToolsUpgradePolicy toggleToolsUpgradePolicy,
    [NotNull] IToggleMksEnable3D toggleMksEnable3D) : IHandleMachineFromPath
{
    private readonly IGuestOsOutputStringMapping _guestOsOutputStringMapping = guestOsOutputStringMapping ??
                                                                               throw new ArgumentNullException(
                                                                                   nameof(guestOsOutputStringMapping));

    private readonly IMachineParserStrategy _machineParserStrategy = machineParserStrategy ?? throw new ArgumentNullException(nameof(machineParserStrategy));
    private readonly IPathSettings _pathSettings = pathSettings ?? throw new ArgumentNullException(nameof(pathSettings));
    private readonly IReadLogInformation _readLogInformation = readLogInformation ?? throw new ArgumentNullException(nameof(readLogInformation));
    private readonly ISetDisplayName _setDisplayName = setDisplayName ?? throw new ArgumentNullException(nameof(setDisplayName));

    private readonly ISetMachineIsEnabledForEditing _setMachineIsEnabledForEditing =
        setMachineIsEnabledForEditing ?? throw new ArgumentNullException(nameof(setMachineIsEnabledForEditing));

    private readonly IToggleToolsSyncTime _toggleToolsSyncTime = toggleToolsSyncTime ?? throw new ArgumentNullException(nameof(toggleToolsSyncTime));
    private readonly IToggleToolsUpgradePolicy _toggleToolsUpgradePolicy = toggleToolsUpgradePolicy ?? throw new ArgumentNullException(nameof(toggleToolsUpgradePolicy));
    private readonly IToggleMksEnable3D _toggleMksEnable3D = toggleMksEnable3D ?? throw new ArgumentNullException(nameof(toggleMksEnable3D));
    private readonly IUpdateMachineVersion _updateMachineVersion = updateMachineVersion ?? throw new ArgumentNullException(nameof(updateMachineVersion));
    private readonly IUpdateMachineMemSize _updateMachineMemSize = updateMachineMemSize ?? throw new ArgumentNullException(nameof(updateMachineMemSize));

    /// <inheritdoc />
    public Machine ValueFor(MachinePath machinePath)
    {
        ArgumentNullException.ThrowIfNull(machinePath);

        var archivePaths = _pathSettings.ArchivePath;
        var machinePoolPath = machinePath.MachinePoolPath;
        var machineFilePath = machinePath.MachineFilePath;

        var fileInfo = machineFilePath.FileInfo();
        if (fileInfo.IsFileLocked() ||
            //has to be done to not handle the archived machines with the non-archived
            archivePaths.Any(archivePath
                                 => !string.IsNullOrWhiteSpace(archivePath) &&
                                    !machinePoolPath.Equals(archivePath, StringComparison.InvariantCultureIgnoreCase) &&
                                    machineFilePath.StartsWith(archivePath, StringComparison.InvariantCultureIgnoreCase)))
        {
            return null;
        }

        var rawMachine = _machineParserStrategy.Parse(machineFilePath);

        //var fileInfo = new FileInfo(machineFilePath);
        var directoryInfo = fileInfo.Directory;
        var (logLastDate, logLastDateDiff) = _readLogInformation.ValueFor(directoryInfo?.FullName);
        var size = directoryInfo.GetDirectorySize();
        var paused = directoryInfo?.GetFiles("*.vmss").Any();
        var properFilePathCapitalization = fileInfo.GetProperFilePathCapitalization();

        var guestOs = rawMachine.MachineType switch
        {
            MachineType.Vmx => rawMachine.GuestOs,
            MachineType.Vbox => rawMachine.OSType,
            _ => string.Empty
        };

        var machine = new Machine(_toggleToolsSyncTime, _toggleToolsUpgradePolicy, _toggleMksEnable3D, _updateMachineVersion, _updateMachineMemSize)
                      {
                          HwVersion = rawMachine.HwVersion,

                          MemSize = rawMachine.MemSize / 1024,
                          ShortPath = properFilePathCapitalization?.Replace(machinePoolPath, "",
                              StringComparison.CurrentCultureIgnoreCase),
                          LogLastDate = logLastDate,
                          LogLastDateDiff = logLastDateDiff,
                          AutoUpdateTools = !string.IsNullOrWhiteSpace(rawMachine.ToolsUpgradePolicy) &&
                                            rawMachine.ToolsUpgradePolicy.Equals("upgradeAtPowerCycle", StringComparison.OrdinalIgnoreCase),
                          SyncTimeWithHost = !string.IsNullOrWhiteSpace(rawMachine.SyncTimeWithHost) &&
                                             bool.TryParse(rawMachine.SyncTimeWithHost, out var parsedSyncTime) && parsedSyncTime,
                          Accelerate3DGraphics = !string.IsNullOrWhiteSpace(rawMachine.MksEnable3D) &&
                                                 bool.TryParse(rawMachine.MksEnable3D, out var parsedAccelerate3DGraphics) && parsedAccelerate3DGraphics,
                          EncryptionData = rawMachine.EncryptionData,
                          EncryptionEncryptedKey = rawMachine.EncryptionEncryptedKey,
                          EncryptionKeySafe = rawMachine.EncryptionKeySafe,
                          ManagedVmAutoAddVTpm = rawMachine.ManagedVmAutoAddVTpm,
                          DisplayName = rawMachine.DisplayName,
                          GuestOs = _guestOsOutputStringMapping.ValueFor(guestOs?.Trim() ?? string.Empty),
                          GuestOsDetailedData = rawMachine.DetailedData,
                          Path = properFilePathCapitalization,
                          Directory = machinePoolPath,
                          DirectorySizeGb = Math.Round(size.KiBiBytesToGiBiBytes(), 2),
                          DirectorySize = size.ToFileSize(2, CultureInfo.GetCultureInfo(1033)),
                          MachineState = paused == true
                              ? MachineState.Paused
                              : MachineState.Off,
                          Annotation = rawMachine.Annotation,
                          MachineType = rawMachine.MachineType
                      };

        _setMachineIsEnabledForEditing.RunFor(machine);
        _setDisplayName.RunFor(rawMachine, machine);
        return machine;
    }
}