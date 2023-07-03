using VmMachineHwVersionUpdater.Core.Commands;

namespace VmMachineHwVersionUpdater.Core.DependencyInjection;

/// <inheritdoc />
public class ConfigureCommandServices : IConfigureCommandServices
{
    /// <inheritdoc />
    public void RunFor([NotNull] IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IGoToCommand, GoToCommand>();
        services.AddSingleton<IOpenWithCodeCommand, OpenWithCodeCommand>();
        services.AddSingleton<IReloadCommand, ReloadCommand>();
        services.AddSingleton<IStartCommand, StartCommand>();
    }
}