using EvilBaschdi.Core.Avalonia;
using VmMachineHwVersionUpdater.Core.Commands;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IReloadReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
public class ReloadReactiveCommand : ReactiveCommandUnitRun, IReloadReactiveCommand
{
    private readonly IReloadCommand _reloadCommand;
    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="reloadCommand"></param>
    /// <param name="mainWindowByApplicationLifetime"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public ReloadReactiveCommand([NotNull] IReloadCommand reloadCommand,
                                 [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime)
    {
        _reloadCommand = reloadCommand ?? throw new ArgumentNullException(nameof(reloadCommand));
        _mainWindowByApplicationLifetime = mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));
    }

    /// <inheritdoc />
    public override async void Run()
    {
        await Task.Run(_reloadCommand.Run);

        _mainWindowByApplicationLifetime.Value?.Close();
    }
}