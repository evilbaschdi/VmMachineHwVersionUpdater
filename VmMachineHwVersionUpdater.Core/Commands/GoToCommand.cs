using EvilBaschdi.Core.AppHelpers;

namespace VmMachineHwVersionUpdater.Core.Commands;

/// <inheritdoc />
public class GoToCommand : IGoToCommand
{
    [NotNull] private readonly IProcessByPath _processByPath;
    [NotNull] private readonly ICurrentItem _currentItem;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="processByPath"></param>
    /// <param name="currentItem"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public GoToCommand([NotNull] IProcessByPath processByPath,
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

        var path = Path.GetDirectoryName(_currentItem.Value.Path);
        if (!string.IsNullOrWhiteSpace(path) && Directory.Exists(path))
        {
            _processByPath.RunFor(path);
        }
    }
}