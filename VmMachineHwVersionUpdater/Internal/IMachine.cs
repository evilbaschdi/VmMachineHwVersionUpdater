namespace VmMachineHwVersionUpdater.Internal
{
    /// <summary>
    /// </summary>
    public interface IMachine
    {
        /// <summary>
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// </summary>
        string DisplayName { get; set; }

        /// <summary>
        /// </summary>
        string ShortPath { get; set; }

        /// <summary>
        /// </summary>
        string Path { get; set; }

        /// <summary>
        /// </summary>
        string DirectorySize { get; set; }

        /// <summary>
        /// </summary>
        double DirectorySizeGb { get; set; }

        /// <summary>
        /// </summary>
        int HwVersion { get; set; }

        /// <summary>
        /// </summary>
        string GuestOs { get; set; }
    }
}