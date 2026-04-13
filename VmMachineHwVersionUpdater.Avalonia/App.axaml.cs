using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using EvilBaschdi.Core.Avalonia.Behaviors;
using EvilBaschdi.Core.Avalonia.DependencyInjection;
using EvilBaschdi.Core.Avalonia.Themes;
using VmMachineHwVersionUpdater.Avalonia.ViewModels;
using VmMachineHwVersionUpdater.Avalonia.Views;

namespace VmMachineHwVersionUpdater.Avalonia;

/// <inheritdoc />
public class App : Application
{
    /// <inheritdoc />
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    /// <inheritdoc />
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            ThemeEngine.Initialize(this);

            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            var mainWindow = new MainWindow
                             {
                                 DataContext = ApplicationServices.GetRequiredService<MainWindowViewModel>()
                             };

            mainWindow.Opened += (sender, _) =>
                                 {
                                     if (sender is not Window window)
                                     {
                                         return;
                                     }

                                     var windowOpenedBehavior = ApplicationServices.ServiceProvider?.GetRequiredService<IWindowOpenedBehavior>();
                                     windowOpenedBehavior?.OnWindowOpened(window);
                                 };

            var handleOsDependentTitleBar = ApplicationServices.GetRequiredService<IHandleOsDependentTitleBar>();
            handleOsDependentTitleBar?.RunFor(mainWindow);

            var applicationLayout = ApplicationServices.GetRequiredService<IApplicationLayout>();
            applicationLayout?.RunFor((mainWindow, true, true));

            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }
}