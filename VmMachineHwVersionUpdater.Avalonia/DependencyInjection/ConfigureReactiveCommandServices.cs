using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

namespace VmMachineHwVersionUpdater.Avalonia.DependencyInjection;

/// <inheritdoc />
public class ConfigureReactiveCommandServices : IConfigureReactiveCommandServices
{
    /// <inheritdoc />
    public void RunFor([NotNull] IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IInitReactiveCommands, InitReactiveCommands>();
        services.AddSingleton<IOpenWithCodeReactiveCommand, OpenWithCodeReactiveCommand>();
        services.AddSingleton<IStartReactiveCommand, StartReactiveCommand>();
    }
}