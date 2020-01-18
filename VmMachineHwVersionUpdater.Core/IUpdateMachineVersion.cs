using System.Collections.Generic;
using EvilBaschdi.Core;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.Core
{
    /// <inheritdoc />
    public interface IUpdateMachineVersion :IRunFor2<List<Machine>, int> ,IRunFor2<string, int>
    {
    }
}