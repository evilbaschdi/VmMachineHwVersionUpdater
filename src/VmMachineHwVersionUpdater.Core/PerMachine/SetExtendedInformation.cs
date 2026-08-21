using System.Text;
using EvilBaschdi.Core.Extensions;

namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public class SetExtendedInformation : ISetExtendedInformation
{
    /// <inheritdoc />
    public void RunFor([NotNull] RawMachine rawMachine, [NotNull] Machine machine)
    {
        ArgumentNullException.ThrowIfNull(rawMachine);
        ArgumentNullException.ThrowIfNull(machine);

        var badges = new List<string>(3);
        var toolTipLines = new List<string>(3);

        if (!string.IsNullOrWhiteSpace(rawMachine.Annotation))
        {
            badges.Add("📄");
            toolTipLines.Add("📄 has Notes");
        }

        if (!string.IsNullOrWhiteSpace(rawMachine.ManagedVmAutoAddVTpm))
        {
            badges.Add("🔐");
            toolTipLines.Add("🔐 has ManagedVm.AutoAddVTpm");
        }

        if (!machine.IsEnabledForEditing)
        {
            badges.Add("🕶");
            toolTipLines.Add("🕶 is currently not enabled for editing");
        }

        var toolTipBuilder = new StringBuilder();

        if (toolTipLines.Count > 0)
        {
            toolTipBuilder.AppendLine(string.Join(Environment.NewLine, toolTipLines));
        }

        var hasGuestData = machine.ParsedGuestInfoDetailedData.Count > 0;

        if (toolTipLines.Count > 0 && hasGuestData)
        {
            toolTipBuilder.AppendLine();
            toolTipBuilder.AppendLine("---");
            toolTipBuilder.AppendLine();
        }

        if (hasGuestData)
        {
            toolTipBuilder.Append(machine.ParsedGuestInfoDetailedData.ToJoinedString(Environment.NewLine, ": "));
        }

        machine.ExtendedInformation = string.Join(" ", badges);
        machine.ExtendedInformationToolTip = toolTipBuilder.ToString().Trim();
    }
}