using EvilBaschdi.Core.Wpf;
using VmMachineHwVersionUpdater.Core.BasicApplication;
using VmMachineHwVersionUpdater.ViewModels.Internal;

namespace VmMachineHwVersionUpdater.DependencyInjection;

/// <inheritdoc />
public class ConfigureWpfServices : IConfigureWpfServices
{
    /// <inheritdoc />
    public void RunFor([NotNull] IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IApplicationStyle>(new ApplicationStyle(true, true));
        services.AddSingleton<IConfigureListCollectionView, ConfigureListCollectionView>();

        services.AddSingleton<ICurrentItemSource, CurrentItemSource>();
        services.AddSingleton<IFilterListCollectionView, FilterListCollectionView>();
        services.AddSingleton<IInitDefaultCommands, InitDefaultCommands>();
        services.AddTransient<ISeparator, SystemWindowsControlsSeparator>();
        services.AddSingleton<ITaskbarItemProgressState, CurrentTaskbarItemProgressState>();
    }
}