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

        if (string.IsNullOrWhiteSpace(newDirectoryName) ||
            newDirectoryName is "." or ".." ||
            Path.IsPathRooted(newDirectoryName) ||
            newDirectoryName.IndexOfAny(['/', '\\']) >= 0)
        {
            throw new ArgumentException("The new directory name must be a single directory name.", nameof(newDirectoryName));
        }

        if (!File.Exists(machine.Path))
        {
            return;
        }

        var sourcePath = Path.GetDirectoryName(machine.Path);

        if (string.IsNullOrWhiteSpace(sourcePath))
        {
            return;
        }

        var relativeMachinePath = Path.GetRelativePath(machine.Directory, sourcePath);
        if (relativeMachinePath == "." || !PathContainment.IsSameOrDescendantOf(machine.Directory, sourcePath))
        {
            return;
        }

        var copyPath = Path.Combine(machine.Directory, newDirectoryName);
        var pathComparison = OperatingSystem.IsWindows()
                                 ? StringComparison.OrdinalIgnoreCase
                                 : StringComparison.Ordinal;
        if (string.Equals(Path.GetFullPath(sourcePath), Path.GetFullPath(copyPath), pathComparison))
        {
            return;
        }

        await _copyDirectory.RunForAsync(sourcePath, copyPath, cancellationToken);
    }
}