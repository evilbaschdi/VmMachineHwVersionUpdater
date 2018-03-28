using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace VmMachineHwVersionUpdater.Internal
{
    /// <inheritdoc />
    public class GuestOsOutputStringMapping : IGuestOsOutputStringMapping
    {
        /// <inheritdoc />
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
            var value = !string.IsNullOrWhiteSpace(fullName) ? fullName : guestOs;

            var os = value.Contains(" ") ? value.Split(' ')[0] : value;

            if (!Value.Contains(os, StringComparer.InvariantCultureIgnoreCase))
            {
                Value.Add(os);
            }


            return value;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Lists all guestOs "short names" (windows, debian, ubuntu, etc.) provided by ValueFor in the current instance.
        /// </summary>
        public List<string> Value { get; } = new List<string>().ToList();
    }
}