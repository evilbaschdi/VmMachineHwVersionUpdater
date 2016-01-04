using EvilBaschdi.Core.Application;

namespace VmMachineHwVersionUpdater.Core
{
    /// <summary>
    /// </summary>
    public class CoreSettings : ISettings
    {
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