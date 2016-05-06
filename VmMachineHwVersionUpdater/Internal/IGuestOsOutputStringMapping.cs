namespace VmMachineHwVersionUpdater.Internal
{
    /// <summary>
    /// </summary>
    public interface IGuestOsOutputStringMapping
    {
        /// <summary>
        ///     Reads guestOs name string from app.config.
        /// </summary>
        string GetGuestOsFullName(string guestOs);
    }
}