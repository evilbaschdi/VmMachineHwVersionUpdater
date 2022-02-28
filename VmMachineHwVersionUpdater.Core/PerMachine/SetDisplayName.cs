using System.Text;
using JetBrains.Annotations;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public class SetDisplayName : ISetDisplayName
{
    /// <inheritdoc />
    public void RunFor([NotNull] RawMachine rawMachine, [NotNull] Machine machine)
    {
        if (rawMachine == null)
        {
            throw new ArgumentNullException(nameof(rawMachine));
        }

        if (machine == null)
        {
            throw new ArgumentNullException(nameof(machine));
        }

        var displayNameBuilder = new StringBuilder(rawMachine.DisplayName.Trim());
        displayNameBuilder.Append(!string.IsNullOrWhiteSpace(rawMachine.Annotation) ? " 📄" : "");
        displayNameBuilder.Append(!string.IsNullOrWhiteSpace(rawMachine.ManagedVmAutoAddVTpm) ? " 🔐" : "");
        displayNameBuilder.Append(!machine.IsEnabledForEditing ? " 🕶" : "");

        machine.DisplayName = displayNameBuilder.ToString().Trim();
    }
}