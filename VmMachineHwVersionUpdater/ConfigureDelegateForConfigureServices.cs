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
    public void RunFor([NotNull] HostBuilderContext _, IServiceCollection services)
    {
        if (_ == null)
        {
            throw new ArgumentNullException(nameof(_));
        }

        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddSingleton(_ => DialogCoordinator.Instance);

        IConfigureWpfServices configureWpfServices = new ConfigureWpfServices();
        configureWpfServices.RunFor(services);

        IConfigureCoreServices configureCoreServices = new ConfigureCoreServices();
        configureCoreServices.RunFor(services);

        IConfigureDefaultCommandServices configureDefaultCommandServices = new ConfigureDefaultCommandServices();
        configureDefaultCommandServices.RunFor(services);

        IConfigureWindowsAndViewModels configureWindowsAndViewModels = new ConfigureWindowsAndViewModels();
        configureWindowsAndViewModels.RunFor(services);
    }
}