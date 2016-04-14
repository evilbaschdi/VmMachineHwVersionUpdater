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

        /// <summary>
        ///     MahApps ThemeManager Accent.
        /// </summary>
        string Accent { get; set; }

        /// <summary>
        ///     MahApps ThemeManager Theme.
        /// </summary>
        string Theme { get; set; }
    }
}