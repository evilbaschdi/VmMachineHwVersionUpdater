using System.Windows.Controls;

namespace VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

/// <inheritdoc />
public class SystemWindowsControlsSeparator : ISeparator
{
    /// <inheritdoc />
    public object Value => new Separator();
}