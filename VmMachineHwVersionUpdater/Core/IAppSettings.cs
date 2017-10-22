namespace VmMachineHwVersionUpdater.Core
{
    /// <summary>
    ///     Wrapper for default settings
    /// </summary>
    public interface IAppSettings
    {
        /// <summary>
        ///     Path of VMware machines
        /// </summary>
        string VMwarePool { get; set; }

        /// <summary>
        ///     Path of VMware machine archive
        /// </summary>
        string ArchivePath { get; set; }
    }
}