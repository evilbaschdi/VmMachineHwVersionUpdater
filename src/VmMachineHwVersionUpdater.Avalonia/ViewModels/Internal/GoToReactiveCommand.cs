namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IGoToReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandRxVoidTask" />
public class GoToReactiveCommand(
    [NotNull] IGoToCommand goToCommand) : ReactiveCommandRxVoidTask, IGoToReactiveCommand
{
    private readonly IGoToCommand _goToCommand = goToCommand ?? throw new ArgumentNullException(nameof(goToCommand));

    /// <inheritdoc />
    public override async Task RunAsync(CancellationToken cancellationToken = default)
    {
        await Task.Run(_goToCommand.Run, cancellationToken);
    }
}