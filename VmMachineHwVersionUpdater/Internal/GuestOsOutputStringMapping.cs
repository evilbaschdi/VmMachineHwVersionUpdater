using System;
using System.Collections.Generic;
using System.Linq;
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

            var os = value.Contains(" ") ? value.Split(' ')[0] : value;

            if (!InnerValue.Contains(os, StringComparer.InvariantCultureIgnoreCase))
            {
                InnerValue.Add(os);
            }

            return value;
        }

        /// <inheritdoc />
        /// <summary>
        ///     Lists all guestOs "short names" (windows, debian, ubuntu, etc.) provided by ValueFor in the current instance.
        /// </summary>
        public List<string> Value { get; } = InnerValue;
    }
}