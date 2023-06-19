using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using EvilBaschdi.About.Avalonia;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IAboutWindowReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
public class AboutWindowReactiveCommand : ReactiveCommandUnitRun, IAboutWindowReactiveCommand
{
    /// <inheritdoc />
    public override void Run()
    {
        var aboutWindow = App.ServiceProvider.GetRequiredService<AboutWindow>();
        var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;
        if (mainWindow != null)
        {
            aboutWindow.ShowDialog(mainWindow);
        }
    }
}