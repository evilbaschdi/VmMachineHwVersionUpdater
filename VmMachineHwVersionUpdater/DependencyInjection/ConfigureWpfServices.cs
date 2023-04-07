using EvilBaschdi.Core.AppHelpers;
using EvilBaschdi.Core.Internal;
using EvilBaschdi.Core.Wpf;
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
        services.AddSingleton<ICopyDirectoryWithFilesWithProgress, CopyDirectoryWithFilesWithProgress>();
        services.AddSingleton<ICopyDirectoryWithProgress, CopyDirectoryWithProgress>();
        services.AddSingleton<ICopyProgress, CopyProgress>();
        services.AddSingleton<ICurrentItemSource, CurrentItemSource>();
        services.AddSingleton<IFilterItemSource, FilterItemSource>();
        services.AddSingleton<IInitDefaultCommands, InitDefaultCommands>();
        services.AddSingleton<ILoadSearchOsItems, LoadSearchOsItems>();
        services.AddSingleton<IProcessByPath, ProcessByPath>();
        services.AddSingleton<ITaskbarItemProgressState, CurrentTaskbarItemProgressState>();
    }
}