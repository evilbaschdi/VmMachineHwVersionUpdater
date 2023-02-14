using EvilBaschdi.Settings;

namespace VmMachineHwVersionUpdater.Core.Settings;

/// <inheritdoc cref="SettingsFromJsonFile" />
public class VmPools : SettingsFromJsonFile, IVmPools
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public VmPools()
        : base("Settings\\VmPools.json")
    {
    }
}