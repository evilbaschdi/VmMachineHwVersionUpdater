using VmMachineHwVersionUpdater.Core.Enums;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public class SetMachineIsEnabledForEditing : ISetMachineIsEnabledForEditing
{
    /// <inheritdoc />
    public void RunFor([NotNull] Machine machine)
    {
        ArgumentNullException.ThrowIfNull(machine);

        if (machine.MachineState == MachineState.Paused ||
            !string.IsNullOrWhiteSpace(machine.EncryptionKeySafe)
            && !string.IsNullOrWhiteSpace(machine.EncryptionData)
            && string.IsNullOrWhiteSpace(machine.GuestOs))
        {
            machine.IsEnabledForEditing = false;
        }
        else
        {
            machine.IsEnabledForEditing = true;
        }
    }
}