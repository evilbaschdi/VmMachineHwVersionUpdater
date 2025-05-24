using VmMachineHwVersionUpdater.Core.Commands;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IGoToReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitTask" />
public class GoToReactiveCommand(
    [NotNull] IGoToCommand goToCommand) : ReactiveCommandUnitTask, IGoToReactiveCommand
{
    private readonly IGoToCommand _goToCommand = goToCommand ?? throw new ArgumentNullException(nameof(goToCommand));

    /// <inheritdoc />
    public override async Task RunAsync()
    {
        await Task.Run(_goToCommand.Run);
    }
}