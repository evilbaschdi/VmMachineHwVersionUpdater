using EvilBaschdi.About.Avalonia;
using EvilBaschdi.Core.Avalonia;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IAboutWindowReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
public class AboutWindowReactiveCommand(
    [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime) : ReactiveCommandUnitRun, IAboutWindowReactiveCommand
{
    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime =
        mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));

    /// <inheritdoc />
    public override void Run()
    {
        var aboutWindow = App.ServiceProvider.GetRequiredService<AboutWindow>();
        var mainWindow = _mainWindowByApplicationLifetime.Value;
        if (mainWindow != null)
        {
            aboutWindow.ShowDialog(mainWindow);
        }
    }
}