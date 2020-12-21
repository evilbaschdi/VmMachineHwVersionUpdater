using EvilBaschdi.Core;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc cref="IMachine" />
    public interface ISelectedMachine : IWritableValue<Machine>, IMachine
    {
    }
}