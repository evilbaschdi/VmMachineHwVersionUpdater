namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public interface ILineStartActions : IValue<Dictionary<string, Action<RawMachine, string>>>
{
}