using EvilBaschdi.Core.AppHelpers;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IOpenWithCodeReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
public class OpenWithCodeReactiveCommand : ReactiveCommandUnitRun, IOpenWithCodeReactiveCommand
{
    private readonly IProcessByPath _processByPath;
    private readonly ICurrentItem _currentItem;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="processByPath"></param>
    /// <param name="currentItem"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public OpenWithCodeReactiveCommand([NotNull] IProcessByPath processByPath,
                                       [NotNull] ICurrentItem currentItem)
    {
        _processByPath = processByPath ?? throw new ArgumentNullException(nameof(processByPath));
        _currentItem = currentItem ?? throw new ArgumentNullException(nameof(currentItem));
    }

    /// <summary>
    ///     Starts VM
    /// </summary>
    public override void Run()
    {
        if (!File.Exists(_currentItem.Value.Path))
        {
            return;
        }

        _processByPath.RunFor($"vscode://file/{_currentItem.Value.Path}");
    }
}