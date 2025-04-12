using System.Collections.Concurrent;

namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <inheritdoc cref="IValue{T}" />
public interface IGuestOsesInUse : IValue<ConcurrentDictionary<string, bool>>;