using VmMachineHwVersionUpdater.Core.Commands;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IStartReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
public class StartReactiveCommand : ReactiveCommandUnitRun, IStartReactiveCommand
{
    private readonly IStartCommand _startCommand;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="startCommand"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public StartReactiveCommand([NotNull] IStartCommand startCommand)
    {
        _startCommand = startCommand ?? throw new ArgumentNullException(nameof(startCommand));
    }

    /// <summary>
    ///     Starts VM
    /// </summary>
    public override void Run()
    {
        _startCommand.Run();
    }
}