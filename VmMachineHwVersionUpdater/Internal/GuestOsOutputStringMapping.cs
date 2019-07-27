using System;
using System.Collections.Generic;
using VmMachineHwVersionUpdater.Core;

namespace VmMachineHwVersionUpdater.Internal
{
    /// <inheritdoc />
    public class GuestOsOutputStringMapping : IGuestOsOutputStringMapping
    {
        private static readonly List<string> InnerValue = new List<string>();

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

            var configuration = GuestOsStringMapping.AppSetting;
            var fullName = configuration[guestOs];
            var value = !string.IsNullOrWhiteSpace(fullName) ? fullName : guestOs;
            return value;
        }
    }
}