using EvilBaschdi.About.Core.DependencyInjection;
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

        IConfigureWpfServices configureWpfServices = new ConfigureWpfServices();
        configureWpfServices.RunFor(serviceCollection);

        IConfigureAboutServices configureAboutServices = new ConfigureAboutServices();
        configureAboutServices.RunFor(serviceCollection);

        IConfigureCoreServices configureCoreServices = new ConfigureCoreServices();
        configureCoreServices.RunFor(serviceCollection);

        IConfigureCommandServices configureCommandServices = new ConfigureCommandServices();
        configureCommandServices.RunFor(serviceCollection);

        IConfigureDefaultCommandServices configureDefaultCommandServices = new ConfigureDefaultCommandServices();
        configureDefaultCommandServices.RunFor(serviceCollection);

        IConfigureWindowsAndViewModels configureWindowsAndViewModels = new ConfigureWindowsAndViewModels();
        configureWindowsAndViewModels.RunFor(serviceCollection);
    }
}