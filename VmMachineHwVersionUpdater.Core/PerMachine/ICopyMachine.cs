using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public interface ICopyMachine : ITaskValueFor2<Machine, string>
{
}