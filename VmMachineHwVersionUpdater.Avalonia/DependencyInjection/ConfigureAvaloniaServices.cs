using System.Collections;
using EvilBaschdi.About.Core;
using EvilBaschdi.Core.Avalonia;
using VmMachineHwVersionUpdater.Avalonia.ViewModels;

namespace VmMachineHwVersionUpdater.Avalonia.DependencyInjection;

/// <inheritdoc />
public class ConfigureAvaloniaServices : IConfigureAvaloniaServices
{
    /// <inheritdoc />
    public void RunFor([NotNull] IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<ICurrentAssembly, CurrentAssembly>();
        services.AddSingleton<IAboutContent, AboutContent>();
        services.AddTransient<ISeparator, AvaloniaControlsSeparator>();
        services.AddSingleton<IComparer, MachineComparer>();
        services.AddSingleton<IConfigureDataGridCollectionView, ConfigureDataGridCollectionView>();
        services.AddSingleton<IFilterDataGridCollectionView, FilterDataGridCollectionView>();
    }
}