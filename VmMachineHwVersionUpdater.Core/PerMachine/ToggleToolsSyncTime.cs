namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc cref="IToggleToolsSyncTime" />
public class ToggleToolsSyncTime() : UpsertVmxLine<bool>("tools.syncTime", "TRUE", "FALSE"), IToggleToolsSyncTime;