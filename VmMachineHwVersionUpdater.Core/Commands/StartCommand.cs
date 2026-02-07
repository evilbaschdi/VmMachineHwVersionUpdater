namespace VmMachineHwVersionUpdater.Core.Commands;

/// <inheritdoc />
public class StartCommand(
    [NotNull] IProcessByPath processByPath,
    [NotNull] ICurrentMachine currentMachine) : IStartCommand
{
    [NotNull] private readonly IProcessByPath _processByPath = processByPath ?? throw new ArgumentNullException(nameof(processByPath));
    [NotNull] private readonly ICurrentMachine _currentMachine = currentMachine ?? throw new ArgumentNullException(nameof(currentMachine));

    /// <inheritdoc />
    public void Run()
    {
        var machinePath = _currentMachine.Value?.Path;
        if (!File.Exists(machinePath))
        {
            return;
        }

        _processByPath.RunFor(machinePath);
    }
}