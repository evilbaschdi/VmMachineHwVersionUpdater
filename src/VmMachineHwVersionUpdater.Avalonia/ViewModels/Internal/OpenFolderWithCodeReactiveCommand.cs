namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IOpenWithCodeReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandRxVoidTask" />
public class OpenFolderWithCodeReactiveCommand(
    [NotNull] IOpenFolderWithCodeCommand openFolderWithCodeCommand) : ReactiveCommandRxVoidTask, IOpenFolderWithCodeReactiveCommand
{
    private readonly IOpenFolderWithCodeCommand _openFolderWithCodeCommand = openFolderWithCodeCommand ?? throw new ArgumentNullException(nameof(openFolderWithCodeCommand));

    /// <summary>
    ///     Starts VM
    /// </summary>
    public override async Task RunAsync(CancellationToken cancellationToken = default)
    {
        await _openFolderWithCodeCommand.RunAsync(cancellationToken);
    }
}