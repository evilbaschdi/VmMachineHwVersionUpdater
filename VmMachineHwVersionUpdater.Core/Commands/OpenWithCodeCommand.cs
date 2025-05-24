using EvilBaschdi.Core.AppHelpers;

namespace VmMachineHwVersionUpdater.Core.Commands;

/// <inheritdoc />
public class OpenWithCodeCommand(
    [NotNull] IProcessByPath processByPath,
    [NotNull] ICurrentMachine currentMachine) : IOpenWithCodeCommand
{
    [NotNull] private readonly IProcessByPath _processByPath = processByPath ?? throw new ArgumentNullException(nameof(processByPath));
    [NotNull] private readonly ICurrentMachine _currentMachine = currentMachine ?? throw new ArgumentNullException(nameof(currentMachine));

    /// <inheritdoc />
    public async Task RunAsync()
    {
        if (!File.Exists(_currentMachine.Value.Path))
        {
            await Task.CompletedTask;
        }

        _processByPath.RunFor($"vscode://file/{_currentMachine.Value.Path}");
        await Task.CompletedTask;
    }
}