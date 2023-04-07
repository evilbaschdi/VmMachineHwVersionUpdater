using EvilBaschdi.Core.Internal;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public class CopyMachine : ICopyMachine
{
    private readonly ICopyDirectoryWithProgress _copyDirectory;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="copyDirectory"></param>
    public CopyMachine([NotNull] ICopyDirectoryWithProgress copyDirectory)
    {
        _copyDirectory = copyDirectory ?? throw new ArgumentNullException(nameof(copyDirectory));
    }

    /// <inheritdoc />
    public async Task ValueFor([NotNull] Machine machine, [NotNull] string newDirectoryName)
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
        }

        await _copyDirectory.ValueFor(path, copyPath);
    }
}