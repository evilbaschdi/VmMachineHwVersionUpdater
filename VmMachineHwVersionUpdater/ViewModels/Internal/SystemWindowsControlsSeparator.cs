using System.Windows.Controls;
using VmMachineHwVersionUpdater.Core.BasicApplication;

namespace VmMachineHwVersionUpdater.ViewModels.Internal;

/// <inheritdoc />
public class SystemWindowsControlsSeparator : ISeparator
{
    /// <inheritdoc />
    public object Value => new Separator();
}