using System.Collections.Generic;
using EvilBaschdi.Core;

namespace VmMachineHwVersionUpdater.Core.BasicApplication
{
    /// <inheritdoc cref="IValue{T}" />
    public interface IGuestOsesInUse : IValue<List<string>>
    {
    }
}