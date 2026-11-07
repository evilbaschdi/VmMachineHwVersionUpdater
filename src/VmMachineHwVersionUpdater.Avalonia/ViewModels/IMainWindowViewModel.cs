namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <inheritdoc />
public interface IMainWindowViewModel : IRun
{
    /// <summary>
    ///     Refreshes the machine data shown in the main window.
    /// </summary>
    Task RefreshMachineDataAsync();
}