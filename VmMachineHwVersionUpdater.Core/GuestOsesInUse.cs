using System;
using System.Collections.Generic;
using System.Linq;
using VmMachineHwVersionUpdater.Core.Settings;

namespace VmMachineHwVersionUpdater.Core
{
    /// <inheritdoc />
    public class GuestOsesInUse : IGuestOsesInUse
    {
        /// <inheritdoc />
        public List<string> Value
        {
            get
            {
                var list = new List<string>();
                var configuration = GuestOsStringMapping.AppSetting;
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