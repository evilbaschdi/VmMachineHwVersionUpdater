namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc cref="IRunFor2{TIn1,TIn2}" />
/// <inheritdoc cref="IUpsertVmxLine{TIn2}" />
public interface IUpdateMachineVersion : IRunFor2<ParallelQuery<Machine>, int>, IUpsertVmxLine<int>;