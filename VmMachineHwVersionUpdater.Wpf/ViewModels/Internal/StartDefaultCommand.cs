using VmMachineHwVersionUpdater.Core.Commands;

namespace VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

/// <inheritdoc />
public class StartDefaultCommand : IStartDefaultCommand
{
    private readonly IStartCommand _startCommand;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="startCommand"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public StartDefaultCommand([NotNull] IStartCommand startCommand)
    {
        _startCommand = startCommand ?? throw new ArgumentNullException(nameof(startCommand));
    }

    /// <inheritdoc />
    public DefaultCommand DefaultCommandValue => new()
                                                 {
                                                     Command = new RelayCommand(_ => Run())
                                                 };

    /// <inheritdoc />
    public void Run()
    {
        _startCommand.Run();
    }
}