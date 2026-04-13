using EvilBaschdi.Core.Avalonia.Behaviors;
using EvilBaschdi.Core.Avalonia.Layout;
using EvilBaschdi.Core.Avalonia.Lifetime;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Collections;
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

        services.TryAddSingleton<IHandleOsDependentTitleBar, HandleOsDependentTitleBar>();
        services.TryAddSingleton<IApplicationLayout, ApplicationLayout>();
        services.TryAddSingleton<IMainWindowByApplicationLifetime, MainWindowByApplicationLifetime>();
        services.TryAddSingleton<IWindowOpenedBehavior, WindowOpenedBehavior>();
    }
}