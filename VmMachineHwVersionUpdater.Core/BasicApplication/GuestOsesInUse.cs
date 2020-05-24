using System;
using System.Collections.Generic;
using System.Linq;
using VmMachineHwVersionUpdater.Core.Settings;

namespace VmMachineHwVersionUpdater.Core.BasicApplication
{
    /// <inheritdoc />
    public class GuestOsesInUse : IGuestOsesInUse
    {
        private readonly IGuestOsStringMapping _guestOsStringMapping;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="guestOsStringMapping"></param>
        public GuestOsesInUse(IGuestOsStringMapping guestOsStringMapping)
        {
            _guestOsStringMapping = guestOsStringMapping ?? throw new ArgumentNullException(nameof(guestOsStringMapping));
        }

        /// <inheritdoc />
        public List<string> Value
        {
            get
            {
                var list = new List<string>();
                var configuration = _guestOsStringMapping.Value;
                foreach (var configurationSection in configuration.GetChildren())
                {
                    var os = configurationSection.Value.Contains(" ") ? configurationSection.Value.Split(' ')[0] : configurationSection.Value;

                    if (!list.Contains(os, StringComparer.InvariantCultureIgnoreCase))
                    {
                        list.Add(os);
                    }
                }

                return list.Distinct().OrderBy(x => x).ToList();
            }
        }
    }
}