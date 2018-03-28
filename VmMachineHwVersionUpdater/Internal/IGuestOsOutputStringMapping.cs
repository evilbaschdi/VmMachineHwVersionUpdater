using System.Collections.Generic;
using EvilBaschdi.Core;

namespace VmMachineHwVersionUpdater.Internal
{
    /// <inheritdoc cref="IValueFor{TIn,TOut}" />
    /// <inheritdoc cref="IValue{TOut}" />
    public interface IGuestOsOutputStringMapping : IValueFor<string, string>, IValue<List<string>>
    {
    }
}