using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using EvilBaschdi.Core.Avalonia;
using VmMachineHwVersionUpdater.Avalonia.DependencyInjection;
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
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);
            var mainWindow = new MainWindow
                             {
                                 DataContext = ApplicationServices.GetRequiredService<MainWindowViewModel>()
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