using EvilBaschdi.Core.AppHelpers;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IStartReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
public class StartReactiveCommand : ReactiveCommandUnitRun, IStartReactiveCommand
{
    private readonly IProcessByPath _processByPath;
    private readonly ICurrentItem _currentItem;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="processByPath"></param>
    /// <param name="currentItem"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public StartReactiveCommand([NotNull] IProcessByPath processByPath,
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
        _processByPath.RunFor(_currentItem.Value.Path);
    }
}