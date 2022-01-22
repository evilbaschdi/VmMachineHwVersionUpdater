using System;
using EvilBaschdi.CoreExtended;
using EvilBaschdi.CoreExtended.Controls.About;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using VmMachineHwVersionUpdater.Core.PerMachine;

namespace VmMachineHwVersionUpdater.ViewModels.Internal;

/// <inheritdoc />
public class ConfigureDefaultCommandServices : IConfigureDefaultCommandServices
{
    /// <inheritdoc />
    public void RunFor([NotNull] IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddSingleton<AboutWindow>();
        services.AddSingleton<IAboutContent, AboutContent>();
        services.AddSingleton<IAboutModel, AboutViewModel>();
        services.AddSingleton<IAboutWindowClickDefaultCommand, AboutWindowClickDefaultCommand>();
        services.AddSingleton<IAddEditAnnotation, AddEditAnnotation>();
        services.AddSingleton<IAddEditAnnotationDefaultCommand, AddEditAnnotationDefaultCommand>();
        services.AddSingleton<IArchiveDefaultCommand, ArchiveDefaultCommand>();
        services.AddSingleton<ICopyDefaultCommand, CopyDefaultCommand>();
        services.AddSingleton<ICurrentAssembly, CurrentAssembly>();
        services.AddSingleton<IDeleteDefaultCommand, DeleteDefaultCommand>();
        services.AddSingleton<IGotToDefaultCommand, GotToDefaultCommand>();
        services.AddSingleton<IOpenWithCodeDefaultCommand, OpenWithCodeDefaultCommand>();
        services.AddSingleton<IReloadDefaultCommand, ReloadDefaultCommand>();
        services.AddSingleton<IStartDefaultCommand, StartDefaultCommand>();
        services.AddSingleton<IUpdateAllDefaultCommand, UpdateAllDefaultCommand>();
    }
}