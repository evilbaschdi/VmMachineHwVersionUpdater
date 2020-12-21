using System.Collections.Generic;
using EvilBaschdi.Core;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc />
    public interface ICurrentItemSource : IWritableValue<List<Machine>>
    {
    }
}