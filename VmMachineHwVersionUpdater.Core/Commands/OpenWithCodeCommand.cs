using EvilBaschdi.Core.AppHelpers;

namespace VmMachineHwVersionUpdater.Core.Commands;

/// <inheritdoc />
/// <summary>
///     Constructor
/// </summary>
/// <param name="processByPath"></param>
/// <param name="currentItem"></param>
/// <exception cref="ArgumentNullException"></exception>
public class OpenWithCodeCommand(
    [NotNull] IProcessByPath processByPath,
    [NotNull] ICurrentItem currentItem) : IOpenWithCodeCommand
{
    [NotNull] private readonly IProcessByPath _processByPath = processByPath ?? throw new ArgumentNullException(nameof(processByPath));
    [NotNull] private readonly ICurrentItem _currentItem = currentItem ?? throw new ArgumentNullException(nameof(currentItem));

    /// <inheritdoc />
    public void Run()
    {
        if (!File.Exists(_currentItem.Value.Path))
        {
            return;
        }

        _processByPath.RunFor($"vscode://file/{_currentItem.Value.Path}");
    }
}