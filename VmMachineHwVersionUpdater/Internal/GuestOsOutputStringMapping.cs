namespace VmMachineHwVersionUpdater.Internal
{
    /// <summary>
    /// </summary>
    public class GuestOsOutputStringMapping : IGuestOsOutputStringMapping
    {
        /// <summary>
        ///     Reads guestOs name string from app.config.
        /// </summary>
        public string GetGuestOsFullName(string guestOs)
        {
            var fullName = System.Configuration.ConfigurationManager.AppSettings[guestOs];
            return !string.IsNullOrWhiteSpace(fullName) ? fullName : guestOs;
        }
    }
}