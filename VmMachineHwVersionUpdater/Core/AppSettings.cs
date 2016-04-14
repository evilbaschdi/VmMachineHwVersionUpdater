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

        /// <summary>
        ///     MahApps ThemeManager Accent.
        /// </summary>
        public string Accent
        {
            get
            {
                return string.IsNullOrWhiteSpace(Properties.Settings.Default.Accent)
                    ? ""
                    : Properties.Settings.Default.Accent;
            }
            set
            {
                Properties.Settings.Default.Accent = value;
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        ///     MahApps ThemeManager Theme.
        /// </summary>
        public string Theme
        {
            get
            {
                return string.IsNullOrWhiteSpace(Properties.Settings.Default.Theme)
                    ? ""
                    : Properties.Settings.Default.Theme;
            }
            set
            {
                Properties.Settings.Default.Theme = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}