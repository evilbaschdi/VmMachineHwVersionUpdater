namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc />
public interface IInitReactiveCommands : IRun
{
    /// <inheritdoc cref="IStartReactiveCommand" />
    public IStartReactiveCommand StartReactiveCommand { get; set; }

    /// <inheritdoc cref="IOpenWithCodeReactiveCommand" />
    public IOpenWithCodeReactiveCommand OpenWithCodeReactiveCommand { get; set; }
}