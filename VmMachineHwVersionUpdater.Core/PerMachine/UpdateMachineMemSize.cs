namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc cref="IUpdateMachineMemSize" />
public class UpdateMachineMemSize() : UpsertVmxLine<int>("memsize"), IUpdateMachineMemSize;