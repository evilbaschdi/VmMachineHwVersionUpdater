using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using EvilBaschdi.About.Avalonia.DependencyInjection;
using VmMachineHwVersionUpdater.Avalonia.DependencyInjection;
using VmMachineHwVersionUpdater.Avalonia.ViewModels;
using VmMachineHwVersionUpdater.Avalonia.Views;

namespace VmMachineHwVersionUpdater.Avalonia;

/// <inheritdoc />
public class App : Application
{
    /// <inheritdoc />
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    /// <summary>
    ///     ServiceProvider for DependencyInjection
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public static IServiceProvider ServiceProvider { get; set; }

    /// <inheritdoc />
    public override void OnFrameworkInitializationCompleted()
    {
        IServiceCollection serviceCollection = new ServiceCollection();

        serviceCollection.AddCoreServices();
        serviceCollection.AddAboutServices();
        serviceCollection.AddCommandServices();
        serviceCollection.AddAvaloniaServices();
        serviceCollection.AddReactiveCommandServices();
        serviceCollection.AddWindowsAndViewModels();

        ServiceProvider = serviceCollection.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);
            var mainWindow = new MainWindow
                             {
                                 DataContext = ServiceProvider.GetRequiredService<MainWindowViewModel>()
                             };

            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }
}