using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public interface IParseVmxFile : IValueFor<string, RawMachine>
{
}