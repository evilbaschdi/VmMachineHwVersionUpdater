using VmMachineHwVersionUpdater.Core.Commands;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IOpenWithCodeReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
/// <summary>
///     Constructor
/// </summary>
/// <param name="openWithCodeCommand"></param>
/// <exception cref="ArgumentNullException"></exception>
public class OpenWithCodeReactiveCommand(
    [NotNull] IOpenWithCodeCommand openWithCodeCommand) : ReactiveCommandUnitRun, IOpenWithCodeReactiveCommand
{
    private readonly IOpenWithCodeCommand _openWithCodeCommand = openWithCodeCommand ?? throw new ArgumentNullException(nameof(openWithCodeCommand));

    /// <summary>
    ///     Starts VM
    /// </summary>
    public override void Run()
    {
        _openWithCodeCommand.Run();
    }
}