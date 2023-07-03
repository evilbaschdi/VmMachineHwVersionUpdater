using VmMachineHwVersionUpdater.Core.Commands;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IOpenWithCodeReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
public class OpenWithCodeReactiveCommand : ReactiveCommandUnitRun, IOpenWithCodeReactiveCommand
{
    private readonly IOpenWithCodeCommand _openWithCodeCommand;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="openWithCodeCommand"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public OpenWithCodeReactiveCommand([NotNull] IOpenWithCodeCommand openWithCodeCommand)
    {
        _openWithCodeCommand = openWithCodeCommand ?? throw new ArgumentNullException(nameof(openWithCodeCommand));
    }

    /// <summary>
    ///     Starts VM
    /// </summary>
    public override void Run()
    {
        _openWithCodeCommand.Run();
    }
}