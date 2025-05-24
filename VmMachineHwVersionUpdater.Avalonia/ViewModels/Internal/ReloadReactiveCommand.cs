using EvilBaschdi.Core.Avalonia;
using VmMachineHwVersionUpdater.Core.Commands;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IReloadReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitTask" />
public class ReloadReactiveCommand(
    [NotNull] IReloadCommand reloadCommand,
    [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime) : ReactiveCommandUnitTask, IReloadReactiveCommand
{
    private readonly IReloadCommand _reloadCommand = reloadCommand ?? throw new ArgumentNullException(nameof(reloadCommand));

    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime =
        mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));

    /// <inheritdoc />
    public override async Task RunAsync()
    {
        await Task.Run(_reloadCommand.Run);

        _mainWindowByApplicationLifetime.Value?.Close();
    }
}