namespace VmMachineHwVersionUpdater.Core.Commands;

/// <inheritdoc />
public class StartCommand(
    [NotNull] IProcessByPath processByPath,
    [NotNull] ICurrentMachine currentMachine) : IStartCommand
{
    private readonly IProcessByPath _processByPath = processByPath ?? throw new ArgumentNullException(nameof(processByPath));
    private readonly ICurrentMachine _currentMachine = currentMachine ?? throw new ArgumentNullException(nameof(currentMachine));

    /// <inheritdoc />
    public void Run()
    {
        var machinePath = _currentMachine.Value?.Path;

        if (machinePath == null)
        {
            return;
        }

        if (!File.Exists(machinePath))
        {
            return;
        }

        _processByPath.RunFor(machinePath);
    }
}