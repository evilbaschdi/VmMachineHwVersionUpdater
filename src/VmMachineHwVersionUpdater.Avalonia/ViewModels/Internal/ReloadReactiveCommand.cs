namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IReloadReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandRxVoidTask" />
public class ReloadReactiveCommand(
    [NotNull] IReloadCommand reloadCommand,
    [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime) : ReactiveCommandRxVoidTask, IReloadReactiveCommand
{
    private readonly IReloadCommand _reloadCommand = reloadCommand ?? throw new ArgumentNullException(nameof(reloadCommand));

    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime =
        mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));

    /// <inheritdoc />
    public override async Task RunAsync(CancellationToken cancellationToken = default)
    {
        await Task.Run(_reloadCommand.Run, cancellationToken);

        _mainWindowByApplicationLifetime.Value?.Close();
    }
}