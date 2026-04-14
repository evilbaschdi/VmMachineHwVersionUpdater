using EvilBaschdi.Core.Internal.Copy;

namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public class CopyMachine(
    [NotNull] ICopyDirectoryWithProgress copyDirectory) : ICopyMachine
{
    private readonly ICopyDirectoryWithProgress _copyDirectory = copyDirectory ?? throw new ArgumentNullException(nameof(copyDirectory));

    /// <inheritdoc />
    public async Task RunForAsync([NotNull] Machine machine, [NotNull] string newDirectoryName, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(machine);

        ArgumentNullException.ThrowIfNull(newDirectoryName);

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
        var copyPath = path.ToLower().Replace(machineDirectoryWithoutPath, newDirectoryName, StringComparison.InvariantCultureIgnoreCase);

        if (path.Equals(copyPath))
        {
            //todo: Message
            return;
        }

        await _copyDirectory.RunForAsync(path, copyPath, cancellationToken);
    }
}