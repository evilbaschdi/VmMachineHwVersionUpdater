namespace VmMachineHwVersionUpdater.Core.Models;

/// <inheritdoc cref="IMachine" />
public interface ICurrentMachine : IWritableValue<Machine>, IMachine;