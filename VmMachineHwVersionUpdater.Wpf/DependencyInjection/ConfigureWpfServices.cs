using EvilBaschdi.Core.Wpf;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

namespace VmMachineHwVersionUpdater.Wpf.DependencyInjection;

/// <inheritdoc />
public class ConfigureWpfServices : IConfigureWpfServices
{
    /// <inheritdoc />
    public void RunFor([NotNull] IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IAddEditAnnotation, AddEditAnnotation>();
        services.TryAddSingleton<IApplicationLayout, ApplicationLayout>();
        services.TryAddSingleton<IApplicationStyle, ApplicationStyle>();
        services.TryAddSingleton<IApplyMicaBrush, ApplyMicaBrush>();
        services.AddSingleton<IChangeDisplayName, ChangeDisplayName>();
        services.AddSingleton<IConfigureListCollectionView, ConfigureListCollectionView>();
        services.AddSingleton<ICurrentItemSource, CurrentItemSource>();
        services.AddSingleton<IFilterListCollectionView, FilterListCollectionView>();
        services.AddTransient<ISeparator, SystemWindowsControlsSeparator>();
        services.AddSingleton<ITaskbarItemProgressState, CurrentTaskbarItemProgressState>();
    }
}