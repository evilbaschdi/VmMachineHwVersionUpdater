namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc cref="IToggleToolsSyncTime" />
public class ToggleToolsSyncTime : UpsertVmxLine<bool>, IToggleToolsSyncTime
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public ToggleToolsSyncTime()
        : base("tools.syncTime", "TRUE", "FALSE")
    {
    }
}