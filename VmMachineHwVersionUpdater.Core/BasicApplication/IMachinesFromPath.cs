using System.Collections.Concurrent;

namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <inheritdoc />
public interface IMachinesFromPath : IValue<ConcurrentBag<Machine>>;