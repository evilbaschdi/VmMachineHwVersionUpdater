using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using VmMachineHwVersionUpdater.Avalonia.DependencyInjection;
using VmMachineHwVersionUpdater.Avalonia.ViewModels;
using VmMachineHwVersionUpdater.Avalonia.Views;
using VmMachineHwVersionUpdater.Core.DependencyInjection;

namespace VmMachineHwVersionUpdater.Avalonia;

/// <inheritdoc />
public class App : Application
{
    /// <inheritdoc />
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    /// <inheritdoc />
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            IConfigureCoreServices configureCoreServices = new ConfigureCoreServices();
            IConfigureAvaloniaServices configureAvaloniaServices = new ConfigureAvaloniaServices();
            IConfigureReactiveCommandServices configureReactiveCommandServices = new ConfigureReactiveCommandServices();
            IConfigureWindowsAndViewModels configureWindowsAndViewModels = new ConfigureWindowsAndViewModels();

            configureCoreServices.RunFor(serviceCollection);
            configureAvaloniaServices.RunFor(serviceCollection);
            configureReactiveCommandServices.RunFor(serviceCollection);
            configureWindowsAndViewModels.RunFor(serviceCollection);

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = new MainWindow
                             {
                                 DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>()
                             };

            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }
}