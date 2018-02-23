using System;
using System.Configuration;

namespace VmMachineHwVersionUpdater.Internal
{
    /// <summary>
    /// </summary>
    public class GuestOsOutputStringMapping : IGuestOsOutputStringMapping
    {
        /// <summary>
        ///     Reads guestOs name string from app.config.
        /// </summary>
        public string ValueFor(string guestOs)
        {
            if (guestOs == null)
            {
                throw new ArgumentNullException(nameof(guestOs));
            }

            var fullName = ConfigurationManager.AppSettings[guestOs];
            return !string.IsNullOrWhiteSpace(fullName) ? fullName : guestOs;
        }
    }
}