using VmMachineHwVersionUpdater.Core.Commands;

namespace VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

/// <inheritdoc />
/// <summary>
///     Constructor
/// </summary>
/// <param name="goToCommand"></param>
/// <exception cref="ArgumentNullException"></exception>
public class GotToDefaultCommand([NotNull] IGoToCommand goToCommand) : IGotToDefaultCommand
{
    private readonly IGoToCommand _goToCommand = goToCommand ?? throw new ArgumentNullException(nameof(goToCommand));

    /// <inheritdoc />
    public DefaultCommand DefaultCommandValue => new()
                                                 {
                                                     Command = new RelayCommand(_ => Run())
                                                 };

    /// <inheritdoc />
    public void Run()
    {
        _goToCommand.Run();
    }
}