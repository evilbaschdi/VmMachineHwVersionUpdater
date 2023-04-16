using EvilBaschdi.About.Core;
using EvilBaschdi.Core.Wpf;
using VmMachineHwVersionUpdater.Core.BasicApplication;
using VmMachineHwVersionUpdater.Core.PerMachine;
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
        services.AddTransient<ISeparator, SystemWindowsControlsSeparator>();
        services.AddSingleton<ITaskbarItemProgressState, CurrentTaskbarItemProgressState>();
        services.AddSingleton<IAboutContent, AboutContent>();
        services.AddSingleton<IApplyMicaBrush, ApplyMicaBrush>();
        services.AddSingleton<IAddEditAnnotation, AddEditAnnotation>();
        services.AddSingleton<IChangeDisplayName, ChangeDisplayName>();
        services.AddSingleton<ICurrentAssembly, CurrentAssembly>();
    }
}