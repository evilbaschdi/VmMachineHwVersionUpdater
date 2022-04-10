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

        var extendedInformationBuilder = new StringBuilder();    
        var extendedInformationToolTipBuilder = new StringBuilder();   

        if(!string.IsNullOrWhiteSpace(rawMachine.Annotation))
        {
             extendedInformationBuilder.Append(" 📄");
             extendedInformationToolTipBuilder.Append(" has Annotation,");
        }

        if(!string.IsNullOrWhiteSpace(rawMachine.ManagedVmAutoAddVTpm))
        {
             extendedInformationBuilder.Append(" 🔐");
             extendedInformationToolTipBuilder.Append(" has ManagedVmAutoAddVTpm,");
        }

        if(!machine.IsEnabledForEditing)
        {
             extendedInformationBuilder.Append(" 🕶");
             extendedInformationToolTipBuilder.Append(" is currently not enabled for editing");
        }
        
        machine.ExtendedInformation = extendedInformationBuilder.ToString().Trim();
        machine.ExtendedInformationToolTip = extendedInformationToolTipBuilder.ToString().Trim().Trim(',');
    }
}