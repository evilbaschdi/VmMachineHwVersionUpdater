namespace VmMachineHwVersionUpdater.Core
{
    /// <summary>
    ///     Wrapper arround Default Settings.
    /// </summary>
    public interface IAppSettings
    {
        /// <summary>
        ///     Path of Vmware machines.
        /// </summary>
        string VMwarePool { get; set; }
        
    }
}