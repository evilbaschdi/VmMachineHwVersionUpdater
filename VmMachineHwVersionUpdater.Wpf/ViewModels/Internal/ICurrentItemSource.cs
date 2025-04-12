using System.Collections.Concurrent;

namespace VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

/// <inheritdoc cref="IWritableValue{T}" />
/// <inheritdoc cref="ICachedValue{T}" />
public interface ICurrentItemSource : IWritableValue<ConcurrentBag<Machine>>, ICachedValue<ConcurrentBag<Machine>>;