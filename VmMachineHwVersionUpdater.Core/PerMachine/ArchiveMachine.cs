namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
/// <summary>
///     Constructor
/// </summary>
/// <param name="pathSettings"></param>
public class ArchiveMachine([NotNull] IPathSettings pathSettings) : IArchiveMachine
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

        var machineDirectoryWithoutPath = path.ToLower().Replace($@"{machine.Directory}\", "", StringComparison.OrdinalIgnoreCase);

        var archivePath = _pathSettings.ArchivePath?.FirstOrDefault(p => p.StartsWith(machine.Directory, StringComparison.OrdinalIgnoreCase));
        archivePath = string.IsNullOrWhiteSpace(archivePath) ? Path.Combine(machine.Directory.ToLower(), "_archive") : archivePath;

        var destination = Path.Combine(archivePath, machineDirectoryWithoutPath.ToLower());
        Directory.Move(path.ToLower(), destination.ToLower());
    }
}