namespace VmMachineHwVersionUpdater.Core.Commands;

/// <inheritdoc />
public class GoToCommand(
    [NotNull] IProcessByPath processByPath,
    [NotNull] ICurrentMachine currentMachine) : IGoToCommand
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

        var path = Path.GetDirectoryName(machinePath);
        if (!string.IsNullOrWhiteSpace(path) && Directory.Exists(path))
        {
            _processByPath.RunFor(path);
        }
    }
}