using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
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
            configureCoreServices.RunFor(serviceCollection);
            serviceCollection.AddSingleton<MainWindowViewModel>();

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            desktop.MainWindow = new MainWindow
                                 {
                                     DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>()
                                 };
        }

        base.OnFrameworkInitializationCompleted();
    }
}