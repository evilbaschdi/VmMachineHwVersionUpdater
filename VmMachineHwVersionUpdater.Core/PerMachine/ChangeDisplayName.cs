namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc cref="IAddEditAnnotation" />
public class ChangeDisplayName() : UpsertVmxLine<string>("displayName"), IChangeDisplayName;