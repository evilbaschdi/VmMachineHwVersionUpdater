using EvilBaschdi.Core.Extensions;

namespace VmMachineHwVersionUpdater.Core.Commands;

/// <inheritdoc />
public class OpenFolderWithCodeCommand(
    [NotNull] IProcessByPath processByPath,
    [NotNull] ICurrentMachine currentMachine) : IOpenFolderWithCodeCommand
{
    private readonly IProcessByPath _processByPath = processByPath ?? throw new ArgumentNullException(nameof(processByPath));
    private readonly ICurrentMachine _currentMachine = currentMachine ?? throw new ArgumentNullException(nameof(currentMachine));

    /// <inheritdoc />
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var machinePath = _currentMachine.Value?.Path.FileInfo().DirectoryName;

        if (machinePath == null)
        {
            return;
        }

        if (!Path.Exists(machinePath))
        {
            await Task.CompletedTask;
        }

        _processByPath.RunFor($"vscode://file/{machinePath}");
        await Task.CompletedTask;
    }
}