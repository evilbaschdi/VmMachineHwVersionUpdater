namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc cref="IAddEditAnnotation" />
public class AddEditAnnotation : UpsertVmxLine<string>, IAddEditAnnotation
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public AddEditAnnotation()
        : base("annotation")
    {
    }
}