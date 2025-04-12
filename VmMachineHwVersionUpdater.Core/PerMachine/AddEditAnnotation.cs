namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc cref="IAddEditAnnotation" />
public class AddEditAnnotation() : UpsertVmxLine<string>("annotation"), IAddEditAnnotation;