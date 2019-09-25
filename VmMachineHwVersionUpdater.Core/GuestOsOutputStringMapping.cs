using System;
using EvilBaschdi.Core;

namespace VmMachineHwVersionUpdater.Core
{
    /// <inheritdoc cref="IGuestOsOutputStringMapping" />
    public class GuestOsOutputStringMapping : CachedValueFor<string, string>, IGuestOsOutputStringMapping
    {
        /// <inheritdoc />
        /// <summary>
        ///     Reads guestOs name string from app.config.
        /// </summary>
        protected override string NonCachedValueFor(string guestOs)
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