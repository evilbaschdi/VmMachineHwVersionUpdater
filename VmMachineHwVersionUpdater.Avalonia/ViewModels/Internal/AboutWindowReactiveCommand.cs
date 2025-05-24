using EvilBaschdi.About.Avalonia;
using EvilBaschdi.Core.Avalonia;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IAboutWindowReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitTask" />
public class AboutWindowReactiveCommand(
    [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime) : ReactiveCommandUnitTask, IAboutWindowReactiveCommand
{
    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime =
        mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));

    /// <inheritdoc />
    public override async Task RunAsync()
    {
        var aboutWindow = App.ServiceProvider.GetRequiredService<AboutWindow>();
        var mainWindow = _mainWindowByApplicationLifetime.Value;
        if (mainWindow != null)
        {
            await aboutWindow.ShowDialog(mainWindow);
        }
    }
}