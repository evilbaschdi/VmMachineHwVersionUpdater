namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc cref="IToggleToolsSyncTime" />
public class ToggleToolsUpgradePolicy() : UpsertVmxLine<bool>("tools.upgrade.policy", "upgradeAtPowerCycle", "useGlobal"), IToggleToolsUpgradePolicy;