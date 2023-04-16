using System.Collections;
using EvilBaschdi.About.Core;
using EvilBaschdi.Core;
using EvilBaschdi.Core.Avalonia;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using VmMachineHwVersionUpdater.Avalonia.ViewModels;
using VmMachineHwVersionUpdater.Core.BasicApplication;

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