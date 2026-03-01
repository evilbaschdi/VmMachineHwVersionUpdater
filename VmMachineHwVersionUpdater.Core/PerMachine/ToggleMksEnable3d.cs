namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc cref="IToggleMksEnable3d" />
public class ToggleMksEnable3d() : UpsertVmxLine<bool>("mks.enable3d", "TRUE", "FALSE"), IToggleMksEnable3d;