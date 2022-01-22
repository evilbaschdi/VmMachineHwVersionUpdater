using System.Collections.Generic;
using EvilBaschdi.Core;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <inheritdoc />
public interface IMachinesFromPath : IValue<List<Machine>>
{
}