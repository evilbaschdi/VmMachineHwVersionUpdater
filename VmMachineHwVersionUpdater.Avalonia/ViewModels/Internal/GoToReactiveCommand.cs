using VmMachineHwVersionUpdater.Core.Commands;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IGoToReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
/// <summary>
///     Constructor
/// </summary>
/// <param name="goToCommand"></param>
/// <exception cref="ArgumentNullException"></exception>
public class GoToReactiveCommand([NotNull] IGoToCommand goToCommand) : ReactiveCommandUnitRun, IGoToReactiveCommand
{
    private readonly IGoToCommand _goToCommand = goToCommand ?? throw new ArgumentNullException(nameof(goToCommand));

    /// <inheritdoc />
    public override void Run()
    {
        _goToCommand.Run();
    }
}