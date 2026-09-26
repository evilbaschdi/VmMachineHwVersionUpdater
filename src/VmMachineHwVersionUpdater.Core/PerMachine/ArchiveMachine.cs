namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public class ArchiveMachine(
    [NotNull] IPathSettings pathSettings) : IArchiveMachine
{
    private readonly IPathSettings _pathSettings = pathSettings ?? throw new ArgumentNullException(nameof(pathSettings));

    /// <inheritdoc />
    public void RunFor([NotNull] Machine machine)
    {
        ArgumentNullException.ThrowIfNull(machine);

        if (!File.Exists(machine.Path))
        {
            return;
        }

        var path = Path.GetDirectoryName(machine.Path);

        if (string.IsNullOrWhiteSpace(path))
        {
            return;
        }

        var relativeMachinePath = Path.GetRelativePath(machine.Directory, path);
        if (relativeMachinePath == "." || !PathContainment.IsSameOrDescendantOf(machine.Directory, path))
        {
            return;
        }

        var archivePath = _pathSettings.ArchivePath?.FirstOrDefault(
            candidateArchivePath => PathContainment.IsSameOrDescendantOf(machine.Directory, candidateArchivePath));
        archivePath = string.IsNullOrWhiteSpace(archivePath) ? Path.Combine(machine.Directory, "_archive") : archivePath;

        var destination = Path.Combine(archivePath, relativeMachinePath);
        var fullSourcePath = Path.GetFullPath(path);
        var fullDestinationPath = Path.GetFullPath(destination);
        if (PathContainment.IsSameOrDescendantOf(fullSourcePath, fullDestinationPath))
        {
            return;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(fullDestinationPath)!);
        Directory.Move(fullSourcePath, fullDestinationPath);
    }
}