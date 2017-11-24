using System.IO;

namespace VmMachineHwVersionUpdater.Core
{
    /// <summary>
    ///     Wrapper for Default Settings.
    /// </summary>
    public class AppSettings : IAppSettings
    {
        /// <summary>
        ///     Path of VMware machines
        /// </summary>
        public string VMwarePool
        {
            get => !string.IsNullOrWhiteSpace(Properties.Settings.Default.VMwarePool)
                ? Properties.Settings.Default.VMwarePool
                : "";
            set
            {
                Properties.Settings.Default.VMwarePool = value;
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        ///     Path of VMware machine archive
        /// </summary>
        public string ArchivePath
        {
            get => !string.IsNullOrWhiteSpace(Properties.Settings.Default.ArchivePath)
                ? Properties.Settings.Default.ArchivePath
                : !string.IsNullOrWhiteSpace(Properties.Settings.Default.VMwarePool)
                    ? Path.Combine(Properties.Settings.Default.VMwarePool, "_archive")
                    : "";
            set
            {
                Properties.Settings.Default.ArchivePath = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}