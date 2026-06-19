using Avalonia.Threading;
using Microsoft.Extensions.Logging;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <inheritdoc cref="IUpdateMachineStateFromFile" />
public class UpdateMachineStateFromFile(
    [NotNull] IReadLogInformation readLogInformation,
    [NotNull] ISetMachineIsEnabledForEditing setMachineIsEnabledForEditing,
    [NotNull] ISetExtendedInformation setExtendedInformation,
    [NotNull] ILogger<UpdateMachineStateFromFile> logger) : IUpdateMachineStateFromFile
{
    private readonly IReadLogInformation _readLogInformation =
        readLogInformation ?? throw new ArgumentNullException(nameof(readLogInformation));

    private readonly ISetMachineIsEnabledForEditing _setMachineIsEnabledForEditing =
        setMachineIsEnabledForEditing ?? throw new ArgumentNullException(nameof(setMachineIsEnabledForEditing));

    private readonly ISetExtendedInformation _setExtendedInformation =
        setExtendedInformation ?? throw new ArgumentNullException(nameof(setExtendedInformation));

    private readonly ILogger<UpdateMachineStateFromFile> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc />
    public void UpdateFor(string filePath, LoadHelper loadValue)
    {
        ArgumentNullException.ThrowIfNull(filePath);
        ArgumentNullException.ThrowIfNull(loadValue);

        try
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            var machineDir = Path.GetDirectoryName(filePath);

            if (string.IsNullOrEmpty(machineDir))
            {
                _logger.LogDebug("Cannot determine machine directory for {FilePath}", filePath);
                return;
            }

            var machine = loadValue.VmDataGridItemsSource.FirstOrDefault(m =>
                                                                             string.Equals(Path.GetDirectoryName(m.Path), machineDir, StringComparison.OrdinalIgnoreCase));

            if (machine is null)
            {
                _logger.LogDebug("Machine not found for directory {Directory}", machineDir);
                return;
            }

            Dispatcher.UIThread.Post(() =>
                                     {
                                         try
                                         {
                                             switch (extension)
                                             {
                                                 case ".vmss":
                                                 {
                                                     var vmssFiles = Directory.GetFiles(machineDir, "*.vmss");
                                                     var newState = vmssFiles.Length > 0 ? MachineState.Paused : MachineState.Off;
                                                     machine.MachineState = newState;

                                                     _setMachineIsEnabledForEditing.RunFor(machine);
                                                     var rawMachine = new RawMachine
                                                                      {
                                                                          Annotation = machine.Annotation,
                                                                          ManagedVmAutoAddVTpm = machine.ManagedVmAutoAddVTpm
                                                                      };
                                                     _setExtendedInformation.RunFor(rawMachine, machine);

                                                     _logger.LogDebug("Updated MachineState to {State} for {Directory}", newState, machineDir);
                                                     break;
                                                 }
                                                 case ".log":
                                                     var (logDate, logDiff) = _readLogInformation.ValueFor(machineDir);
                                                     machine.LogLastDate = logDate;
                                                     machine.LogLastDateDiff = logDiff;
                                                     _logger.LogDebug("Updated LogLastDate={LogDate} for {Directory}", logDate, machineDir);
                                                     break;
                                             }
                                         }
                                         catch (Exception ex)
                                         {
                                             _logger.LogError(ex, "Error updating machine state/log for {Directory}", machineDir);
                                         }
                                     });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception in UpdateFor for {FilePath}", filePath);
        }
    }
}