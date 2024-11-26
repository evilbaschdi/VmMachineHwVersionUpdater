using VmMachineHwVersionUpdater.Core.Commands;

namespace VmMachineHwVersionUpdater.Core.DependencyInjection;

/// <summary />
public static class ConfigureCommandServices
{
    /// <summary />
    public static void AddCommandServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IGoToCommand, GoToCommand>();
        services.AddSingleton<IOpenWithCodeCommand, OpenWithCodeCommand>();
        services.AddSingleton<IReloadCommand, ReloadCommand>();
        services.AddSingleton<IStartCommand, StartCommand>();
    }
}