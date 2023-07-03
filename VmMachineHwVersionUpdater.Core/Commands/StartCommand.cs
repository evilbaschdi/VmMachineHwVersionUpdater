using EvilBaschdi.Core.AppHelpers;

namespace VmMachineHwVersionUpdater.Core.Commands;

/// <inheritdoc />
public class StartCommand : IStartCommand
{
    [NotNull] private readonly IProcessByPath _processByPath;
    [NotNull] private readonly ICurrentItem _currentItem;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="processByPath"></param>
    /// <param name="currentItem"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public StartCommand([NotNull] IProcessByPath processByPath,
                        [NotNull] ICurrentItem currentItem)
    {
        _processByPath = processByPath ?? throw new ArgumentNullException(nameof(processByPath));
        _currentItem = currentItem ?? throw new ArgumentNullException(nameof(currentItem));
    }

    /// <inheritdoc />
    public void Run()
    {
        if (!File.Exists(_currentItem.Value.Path))
        {
            return;
        }

        _processByPath.RunFor(_currentItem.Value.Path);
    }
}