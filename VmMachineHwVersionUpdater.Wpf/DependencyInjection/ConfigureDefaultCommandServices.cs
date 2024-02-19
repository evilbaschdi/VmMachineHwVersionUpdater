﻿using VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

namespace VmMachineHwVersionUpdater.Wpf.DependencyInjection;

/// <inheritdoc />
public class ConfigureDefaultCommandServices : IConfigureDefaultCommandServices
{
    /// <inheritdoc />
    public void RunFor([NotNull] IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IInitDefaultCommands, InitDefaultCommands>();

        services.AddSingleton<IAboutWindowClickDefaultCommand, AboutWindowClickDefaultCommand>();
        services.AddSingleton<IAddEditAnnotationDefaultCommand, AddEditAnnotationDefaultCommand>();
        services.AddSingleton<IArchiveDefaultCommand, ArchiveDefaultCommand>();
        services.AddSingleton<ICopyDefaultCommand, CopyDefaultCommand>();
        services.AddSingleton<IDeleteDefaultCommand, DeleteDefaultCommand>();
        services.AddSingleton<IGotToDefaultCommand, GotToDefaultCommand>();
        services.AddSingleton<IOpenWithCodeDefaultCommand, OpenWithCodeDefaultCommand>();
        services.AddSingleton<IReloadDefaultCommand, ReloadDefaultCommand>();
        services.AddSingleton<IRenameDefaultCommand, RenameDefaultCommand>();
        services.AddSingleton<IStartDefaultCommand, StartDefaultCommand>();
        services.AddSingleton<IUpdateAllDefaultCommand, UpdateAllDefaultCommand>();
    }
}