namespace VmMachineHwVersionUpdater.Core.Models;

/// <inheritdoc cref="ICurrentMachine" />
public class CurrentMachine : CachedWritableValue<Machine>, ICurrentMachine
{
    private Machine _machine;

    /// <inheritdoc />
    protected override Machine NonCachedValue => _machine;

    /// <inheritdoc />
    protected override void SaveValue([NotNull] Machine machine)
    {
        _machine = machine;
    }
}