using VmMachineHwVersionUpdater.Core.Commands;

namespace VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

/// <inheritdoc />
public class GotToDefaultCommand : IGotToDefaultCommand
{
    private readonly IGoToCommand _goToCommand;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="goToCommand"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public GotToDefaultCommand([NotNull] IGoToCommand goToCommand)
    {
        _goToCommand = goToCommand ?? throw new ArgumentNullException(nameof(goToCommand));
    }

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