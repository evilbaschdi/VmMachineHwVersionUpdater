using System.Collections.Generic;
using EvilBaschdi.Core;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.ViewModels.Internal;

/// <inheritdoc cref="IWritableValue{T}" />
/// <inheritdoc cref="ICachedValue{T}" />
public interface ICurrentItemSource : IWritableValue<List<Machine>>, ICachedValue<List<Machine>>
{
}