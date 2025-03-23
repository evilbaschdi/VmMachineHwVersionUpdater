namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public interface ILineStartActions : IDictionaryOf<string, Action<RawMachine, string>>;