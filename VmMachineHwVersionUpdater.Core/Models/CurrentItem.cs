namespace VmMachineHwVersionUpdater.Core.Models;

/// <inheritdoc cref="ICurrentItem" />
public class CurrentItem : CachedWritableValue<Machine>, ICurrentItem
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