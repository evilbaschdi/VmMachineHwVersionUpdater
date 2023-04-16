using JetBrains.Annotations;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc />
public class InitReactiveCommands : IInitReactiveCommands
{
    [NotNull] private readonly IOpenWithCodeReactiveCommand _openWithCodeReactiveCommand;
    [NotNull] private readonly IStartReactiveCommand _startReactiveCommand;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// ///
    /// <param name="openWithCodeReactiveCommand"></param>
    /// <param name="startReactiveCommand"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public InitReactiveCommands(
        [NotNull] IOpenWithCodeReactiveCommand openWithCodeReactiveCommand,
        [NotNull] IStartReactiveCommand startReactiveCommand
    )
    {
        _startReactiveCommand = startReactiveCommand ?? throw new ArgumentNullException(nameof(startReactiveCommand));
        _openWithCodeReactiveCommand = openWithCodeReactiveCommand ?? throw new ArgumentNullException(nameof(openWithCodeReactiveCommand));
    }

    /// <inheritdoc />
    public void Run()
    {
        OpenWithCodeReactiveCommand = _openWithCodeReactiveCommand;
        StartReactiveCommand = _startReactiveCommand;
    }

    /// <inheritdoc />
    public IStartReactiveCommand StartReactiveCommand { get; set; }

    /// <inheritdoc />
    public IOpenWithCodeReactiveCommand OpenWithCodeReactiveCommand { get; set; }
}