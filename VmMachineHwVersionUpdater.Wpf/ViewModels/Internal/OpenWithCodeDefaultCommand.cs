using VmMachineHwVersionUpdater.Core.Commands;

namespace VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

/// <inheritdoc />
public class OpenWithCodeDefaultCommand : IOpenWithCodeDefaultCommand
{
    private readonly IOpenWithCodeCommand _openWithCodeCommand;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="openWithCodeCommand"></param>
    public OpenWithCodeDefaultCommand([NotNull] IOpenWithCodeCommand openWithCodeCommand)
    {
        _openWithCodeCommand = openWithCodeCommand ?? throw new ArgumentNullException(nameof(openWithCodeCommand));
    }

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