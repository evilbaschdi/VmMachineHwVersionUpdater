namespace VmMachineHwVersionUpdater.Core
{
    /// <summary>
    ///     Wrapper arround Default Settings.
    /// </summary>
    public class AppSettings : IAppSettings
    {
        /// <summary>
        ///     Path of Vmware machines.
        /// </summary>
        public string VMwarePool
        {
            get
            {
                return string.IsNullOrWhiteSpace(Properties.Settings.Default.VMwarePool)
                    ? ""
                    : Properties.Settings.Default.VMwarePool;
            }
            set
            {
                Properties.Settings.Default.VMwarePool = value;
                Properties.Settings.Default.Save();
            }
        }
        
    }
}