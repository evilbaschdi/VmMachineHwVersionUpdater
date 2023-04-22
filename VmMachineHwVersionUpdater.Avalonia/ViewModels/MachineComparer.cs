using System.Collections;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <inheritdoc />
public class MachineComparer : IComparer
{
    /// <inheritdoc />
    public int Compare(object x, object y)
    {
        var xMachine = (Machine)x;
        var yMachine = (Machine)y;

        return string.Compare(xMachine?.DisplayName, yMachine?.DisplayName, StringComparison.OrdinalIgnoreCase);
    }
}