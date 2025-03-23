namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public interface IReadLogInformation : IValueFor<string, (string LogLastDate, string LogLastDateDiff)>;