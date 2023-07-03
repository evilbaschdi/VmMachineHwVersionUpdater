using VmMachineHwVersionUpdater.Core.Commands;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IGoToReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
public class GoToReactiveCommand : ReactiveCommandUnitRun, IGoToReactiveCommand
{
    private readonly IGoToCommand _goToCommand;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="goToCommand"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public GoToReactiveCommand([NotNull] IGoToCommand goToCommand)
    {
        _goToCommand = goToCommand ?? throw new ArgumentNullException(nameof(goToCommand));
    }

    /// <inheritdoc />
    public override void Run()
    {
        _goToCommand.Run();
    }
}