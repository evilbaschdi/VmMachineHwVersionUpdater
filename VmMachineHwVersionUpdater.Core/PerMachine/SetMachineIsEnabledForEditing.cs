using System;
using JetBrains.Annotations;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public class SetMachineIsEnabledForEditing : ISetMachineIsEnabledForEditing
{
    /// <inheritdoc />
    public void RunFor([NotNull] Machine machine)
    {
        if (machine == null)
        {
            throw new ArgumentNullException(nameof(machine));
        }

        if (!string.IsNullOrWhiteSpace(machine.EncryptionKeySafe)
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