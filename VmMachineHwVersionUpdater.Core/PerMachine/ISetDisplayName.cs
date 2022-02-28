using EvilBaschdi.Core;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public interface ISetDisplayName : IRunFor2<RawMachine, Machine>
{
}