using EvilBaschdi.Core;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.Core.PerMachine
{
    /// <inheritdoc />
    public interface ICopyMachine : IRunForAsync2<Machine, string>
    {
    }
}