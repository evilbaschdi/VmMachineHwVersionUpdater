using EvilBaschdi.DependencyInjection;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Extensions.Hosting;
using VmMachineHwVersionUpdater.Core.DependencyInjection;
using VmMachineHwVersionUpdater.DependencyInjection;

namespace VmMachineHwVersionUpdater;

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

        IConfigureCoreServices configureCoreServices = new ConfigureCoreServices();
        configureCoreServices.RunFor(serviceCollection);

        IConfigureDefaultCommandServices configureDefaultCommandServices = new ConfigureDefaultCommandServices();
        configureDefaultCommandServices.RunFor(serviceCollection);

        IConfigureWindowsAndViewModels configureWindowsAndViewModels = new ConfigureWindowsAndViewModels();
        configureWindowsAndViewModels.RunFor(serviceCollection);
    }
}