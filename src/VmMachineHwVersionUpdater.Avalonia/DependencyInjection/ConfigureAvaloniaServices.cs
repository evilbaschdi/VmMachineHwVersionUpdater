using System.Collections;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VmMachineHwVersionUpdater.Avalonia.ViewModels;

namespace VmMachineHwVersionUpdater.Avalonia.DependencyInjection;

/// <summary />
public static class ConfigureAvaloniaServices
{
    /// <summary />
    public static void AddAvaloniaServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddTransient<ISeparator, AvaloniaControlsSeparator>();
        services.AddSingleton<IComparer, MachineComparer>();
        services.AddSingleton<IConfigureDataGridCollectionView, ConfigureDataGridCollectionView>();
        services.AddSingleton<IFilterDataGridCollectionView, FilterDataGridCollectionView>();
        services.AddSingleton<IVmFileChangeHandler, VmFileChangeHandler>();

        services.TryAddSingleton<IMainWindowByApplicationLifetime, MainWindowByApplicationLifetime>();
    }
}