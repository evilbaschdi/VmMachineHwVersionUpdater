using EvilBaschdi.Core.AppHelpers;

namespace VmMachineHwVersionUpdater.Core.Commands;

/// <inheritdoc />
/// <summary>
///     Constructor
/// </summary>
/// <param name="processByPath"></param>
/// <param name="currentMachine"></param>
/// <exception cref="ArgumentNullException"></exception>
public class GoToCommand(
    [NotNull] IProcessByPath processByPath,
    [NotNull] ICurrentMachine currentMachine) : IGoToCommand
{
    [NotNull] private readonly IProcessByPath _processByPath = processByPath ?? throw new ArgumentNullException(nameof(processByPath));
    [NotNull] private readonly ICurrentMachine _currentMachine = currentMachine ?? throw new ArgumentNullException(nameof(currentMachine));

    /// <inheritdoc />
    public void Run()
    {
        if (!File.Exists(_currentMachine.Value.Path))
        {
            return;
        }

        var path = Path.GetDirectoryName(_currentMachine.Value.Path);
        if (!string.IsNullOrWhiteSpace(path) && Directory.Exists(path))
        {
            _processByPath.RunFor(path);
        }
    }
}