namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc cref="IToggleMksEnable3D" />
public class ToggleMksEnable3D() : UpsertVmxLine<bool>("mks.enable3d", "TRUE", "FALSE"), IToggleMksEnable3D;