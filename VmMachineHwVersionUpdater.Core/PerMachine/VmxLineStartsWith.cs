namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public class VmxLineStartsWith : IVmxLineStartsWith
{
    /// <inheritdoc />
    public bool ValueFor([NotNull] string line, [NotNull] string key)
    {
        ArgumentNullException.ThrowIfNull(line);

        ArgumentNullException.ThrowIfNull(key);

        return line.StartsWith($"{key} =", StringComparison.InvariantCultureIgnoreCase);
    }
}