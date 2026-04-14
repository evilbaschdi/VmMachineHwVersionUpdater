namespace VmMachineHwVersionUpdater.Core.Commands;

/// <inheritdoc />
public class OpenWithCodeCommand(
    [NotNull] IProcessByPath processByPath,
    [NotNull] ICurrentMachine currentMachine) : IOpenWithCodeCommand
{
    [NotNull] private readonly IProcessByPath _processByPath = processByPath ?? throw new ArgumentNullException(nameof(processByPath));
    [NotNull] private readonly ICurrentMachine _currentMachine = currentMachine ?? throw new ArgumentNullException(nameof(currentMachine));

    /// <inheritdoc />
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var machinePath = _currentMachine.Value?.Path;
        if (!File.Exists(machinePath))
        {
            await Task.CompletedTask;
        }

        _processByPath.RunFor($"vscode://file/{machinePath}");
        await Task.CompletedTask;
    }
}