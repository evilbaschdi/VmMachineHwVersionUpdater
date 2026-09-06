namespace VmMachineHwVersionUpdater.Core.Commands;

/// <inheritdoc />
public class OpenWithCodeCommand(
    [NotNull] IProcessByPath processByPath,
    [NotNull] ICurrentMachine currentMachine) : IOpenWithCodeCommand
{
    private readonly IProcessByPath _processByPath = processByPath ?? throw new ArgumentNullException(nameof(processByPath));
    private readonly ICurrentMachine _currentMachine = currentMachine ?? throw new ArgumentNullException(nameof(currentMachine));

    /// <inheritdoc />
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var machinePath = _currentMachine.Value?.Path;

        if (machinePath == null)
        {
            return;
        }

        if (!File.Exists(machinePath))
        {
            await Task.CompletedTask;
        }

        _processByPath.RunFor($"vscode://file/{machinePath}");
        await Task.CompletedTask;
    }
}