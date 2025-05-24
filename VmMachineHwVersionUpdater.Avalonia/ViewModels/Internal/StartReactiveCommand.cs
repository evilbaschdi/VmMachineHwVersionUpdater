using VmMachineHwVersionUpdater.Core.Commands;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IStartReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitTask" />
public class StartReactiveCommand(
    [NotNull] IStartCommand startCommand) : ReactiveCommandUnitTask, IStartReactiveCommand
{
    private readonly IStartCommand _startCommand = startCommand ?? throw new ArgumentNullException(nameof(startCommand));

    /// <summary>
    ///     Starts VM
    /// </summary>
    public override async Task RunAsync()
    {
        await Task.Run(_startCommand.Run);
    }
}