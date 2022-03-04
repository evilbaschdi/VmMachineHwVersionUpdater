using EvilBaschdi.Core;

namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc cref="IRunFor2{TIn1,TIn2}" />
/// <inheritdoc cref="IUpsertVmxLine{TIn2}" />
public interface IChangeDisplayName : IUpsertVmxLine<string>
{
}