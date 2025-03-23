using EvilBaschdi.Core.AppHelpers;

namespace VmMachineHwVersionUpdater.Core.Commands;

/// <inheritdoc />
/// <summary>
///     Constructor
/// </summary>
/// <param name="processByPath"></param>
/// <param name="currentMachine"></param>
/// <exception cref="ArgumentNullException"></exception>
public class StartCommand(
    [NotNull] IProcessByPath processByPath,
    [NotNull] ICurrentMachine currentMachine) : IStartCommand
{
    [NotNull] private readonly IProcessByPath _processByPath = processByPath ?? throw new ArgumentNullException(nameof(processByPath));
    [NotNull] private readonly ICurrentMachine _currentMachine = currentMachine ?? throw new ArgumentNullException(nameof(currentMachine));

    /// <inheritdoc />
    public void Run()
    {
        if (!File.Exists(_currentMachine.Value?.Path))
        {
            return;
        }

        _processByPath.RunFor(_currentMachine.Value?.Path);
    }
}