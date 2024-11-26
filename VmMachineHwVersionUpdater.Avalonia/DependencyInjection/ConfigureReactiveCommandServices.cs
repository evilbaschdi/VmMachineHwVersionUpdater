using VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

namespace VmMachineHwVersionUpdater.Avalonia.DependencyInjection;

/// <summary />
public static class ConfigureReactiveCommandServices
{
    /// <summary />
    public static void AddReactiveCommandServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IInitReactiveCommands, InitReactiveCommands>();

        services.AddSingleton<IAboutWindowReactiveCommand, AboutWindowReactiveCommand>();
        services.AddSingleton<IAddEditAnnotationReactiveCommand, AddEditAnnotationReactiveCommand>();
        services.AddSingleton<IArchiveReactiveCommand, ArchiveReactiveCommand>();
        services.AddSingleton<ICopyReactiveCommand, CopyReactiveCommand>();
        services.AddSingleton<IDeleteReactiveCommand, DeleteReactiveCommand>();
        services.AddSingleton<IGoToReactiveCommand, GoToReactiveCommand>();
        services.AddSingleton<IOpenWithCodeReactiveCommand, OpenWithCodeReactiveCommand>();
        services.AddSingleton<IReloadReactiveCommand, ReloadReactiveCommand>();
        services.AddSingleton<IRenameReactiveCommand, RenameReactiveCommand>();
        services.AddSingleton<IStartReactiveCommand, StartReactiveCommand>();
        services.AddSingleton<IUpdateAllReactiveCommand, UpdateAllReactiveCommand>();
    }
}