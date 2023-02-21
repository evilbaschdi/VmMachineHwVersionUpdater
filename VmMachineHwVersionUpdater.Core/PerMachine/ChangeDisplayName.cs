namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc cref="IAddEditAnnotation" />
public class ChangeDisplayName : UpsertVmxLine<string>, IChangeDisplayName
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public ChangeDisplayName()
        : base("displayName")
    {
    }
}