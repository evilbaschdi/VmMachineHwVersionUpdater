using VmMachineHwVersionUpdater.Core.Models;
using VmMachineHwVersionUpdater.Core.Settings;

namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public class ArchiveMachine : IArchiveMachine
{
    private readonly IPathSettings _pathSettings;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="pathSettings"></param>
    public ArchiveMachine([NotNull] IPathSettings pathSettings)
    {
        _pathSettings = pathSettings ?? throw new ArgumentNullException(nameof(pathSettings));
    }

    /// <inheritdoc />
    public void RunFor([NotNull] Machine machine)
    {
        if (machine == null)
        {
            throw new ArgumentNullException(nameof(machine));
        }

        if (!File.Exists(machine.Path))
        {
            return;
        }

        var path = Path.GetDirectoryName(machine.Path);

        if (string.IsNullOrWhiteSpace(path))
        {
            return;
        }

        var machineDirectoryWithoutPath = path.ToLower().Replace($@"{machine.Directory}\", "", StringComparison.InvariantCultureIgnoreCase);

        var archivePath = _pathSettings.ArchivePath?.FirstOrDefault(p => p.ToLower().StartsWith(machine.Directory.ToLower()));
        archivePath = string.IsNullOrWhiteSpace(archivePath) ? Path.Combine(machine.Directory.ToLower(), "_archive") : archivePath;

        var destination = Path.Combine(archivePath, machineDirectoryWithoutPath.ToLower());
        Directory.Move(path.ToLower(), destination.ToLower());
    }
}