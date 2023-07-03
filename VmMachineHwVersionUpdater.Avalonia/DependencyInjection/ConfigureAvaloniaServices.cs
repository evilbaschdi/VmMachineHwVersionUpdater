using System.Collections;
using EvilBaschdi.Core.Avalonia;
using Microsoft.Extensions.DependencyInjection.Extensions;
using VmMachineHwVersionUpdater.Avalonia.ViewModels;

namespace VmMachineHwVersionUpdater.Avalonia.DependencyInjection;

/// <inheritdoc />
public class ConfigureAvaloniaServices : IConfigureAvaloniaServices
{
    /// <inheritdoc />
    public void RunFor([NotNull] IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddTransient<ISeparator, AvaloniaControlsSeparator>();
        services.AddSingleton<IComparer, MachineComparer>();
        services.AddSingleton<IConfigureDataGridCollectionView, ConfigureDataGridCollectionView>();
        services.AddSingleton<IFilterDataGridCollectionView, FilterDataGridCollectionView>();

        services.TryAddSingleton<IHandleOsDependentTitleBar, HandleOsDependentTitleBar>();
        services.TryAddSingleton<IApplicationLayout, ApplicationLayout>();
        services.TryAddSingleton<IMainWindowByApplicationLifetime, MainWindowsByApplicationLifetime>();
    }
}