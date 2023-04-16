using Avalonia.Controls;
using VmMachineHwVersionUpdater.Core.BasicApplication;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <inheritdoc />
public class AvaloniaControlsSeparator : ISeparator
{
    /// <inheritdoc />
    public object Value => new Separator();
}