using VmMachineHwVersionUpdater.Core.Commands;

namespace VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

/// <inheritdoc />
/// <summary>
///     Constructor
/// </summary>
/// <param name="openWithCodeCommand"></param>
public class OpenWithCodeDefaultCommand([NotNull] IOpenWithCodeCommand openWithCodeCommand) : IOpenWithCodeDefaultCommand
{
    private readonly IOpenWithCodeCommand _openWithCodeCommand = openWithCodeCommand ?? throw new ArgumentNullException(nameof(openWithCodeCommand));

    /// <inheritdoc />
    public DefaultCommand DefaultCommandValue => new()
                                                 {
                                                     Command = new RelayCommand(_ => Run())
                                                 };

    /// <inheritdoc />
    public void Run()
    {
        _openWithCodeCommand.Run();
    }
}