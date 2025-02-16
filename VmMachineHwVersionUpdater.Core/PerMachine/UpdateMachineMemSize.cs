namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc cref="IUpdateMachineMemSize" />
public class UpdateMachineMemSize : UpsertVmxLine<int>, IUpdateMachineMemSize
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public UpdateMachineMemSize()
        : base("memsize")
    {
    }
}