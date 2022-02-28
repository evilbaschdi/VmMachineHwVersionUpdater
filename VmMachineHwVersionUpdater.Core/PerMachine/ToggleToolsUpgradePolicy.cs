namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc cref="IToggleToolsSyncTime" />
public class ToggleToolsUpgradePolicy : UpsertVmxLine<bool>, IToggleToolsUpgradePolicy
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public ToggleToolsUpgradePolicy()
        : base("tools.upgrade.policy", "upgradeAtPowerCycle", "useGlobal")
    {
    }
}