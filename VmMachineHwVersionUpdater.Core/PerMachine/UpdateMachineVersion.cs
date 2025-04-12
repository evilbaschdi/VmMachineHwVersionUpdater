namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc cref="IUpdateMachineVersion" />
public class UpdateMachineVersion() : UpsertVmxLine<int>("virtualhw.version"), IUpdateMachineVersion
{
    /// <inheritdoc />
    public void RunFor(ParallelQuery<Machine> machines, int newVersion)
    {
        ArgumentNullException.ThrowIfNull(machines);

        Parallel.ForEach(machines.Where(m => m.IsEnabledForEditing), machine => { RunFor(machine.Path, newVersion); });
    }
}