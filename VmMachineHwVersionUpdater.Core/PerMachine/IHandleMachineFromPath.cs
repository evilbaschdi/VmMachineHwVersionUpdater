using EvilBaschdi.Core;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public interface IHandleMachineFromPath : IValueFor2<string, string, Machine>
{
}