namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IOpenWithCodeReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandRxVoidTask" />
public class OpenWithCodeReactiveCommand(
    [NotNull] IOpenWithCodeCommand openWithCodeCommand) : ReactiveCommandRxVoidTask, IOpenWithCodeReactiveCommand
{
    private readonly IOpenWithCodeCommand _openWithCodeCommand = openWithCodeCommand ?? throw new ArgumentNullException(nameof(openWithCodeCommand));

    /// <summary>
    ///     Starts VM
    /// </summary>
    public override async Task RunAsync(CancellationToken cancellationToken = default)
    {
        await _openWithCodeCommand.RunAsync(cancellationToken);
    }
}