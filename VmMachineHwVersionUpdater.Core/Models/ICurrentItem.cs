using EvilBaschdi.Core;

namespace VmMachineHwVersionUpdater.Core.Models;

/// <inheritdoc cref="IMachine" />
public interface ICurrentItem : IWritableValue<Machine>, IMachine
{
}