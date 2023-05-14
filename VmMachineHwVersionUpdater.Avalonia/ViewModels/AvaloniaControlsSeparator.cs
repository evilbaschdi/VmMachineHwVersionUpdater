using Avalonia.Controls;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <inheritdoc />
public class AvaloniaControlsSeparator : ISeparator
{
    /// <inheritdoc />
    public object Value => new Separator();
}