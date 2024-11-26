using EvilBaschdi.About.Wpf.DependencyInjection;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Extensions.Hosting;

namespace VmMachineHwVersionUpdater.Wpf.DependencyInjection;

/// <inheritdoc />
public class ConfigureDelegateForConfigureServices : IConfigureDelegateForConfigureServices
{
    /// <inheritdoc />
    public void RunFor([NotNull] HostBuilderContext _, IServiceCollection serviceCollection)
    {
        ArgumentNullException.ThrowIfNull(_);
        ArgumentNullException.ThrowIfNull(serviceCollection);

        serviceCollection.AddSingleton(_ => DialogCoordinator.Instance);

        serviceCollection.AddWpfServices();
        serviceCollection.AddAboutServices();
        serviceCollection.AddCoreServices();
        serviceCollection.AddCommandServices();
        serviceCollection.AddDefaultCommandServices();
        serviceCollection.AddWindowsAndViewModels();
    }
}