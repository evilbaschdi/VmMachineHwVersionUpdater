using EvilBaschdi.Settings;

namespace VmMachineHwVersionUpdater.Core.Settings
{/// <inheritdoc cref="SettingsFromJsonFile" />
    public class GuestOsStringMapping : SettingsFromJsonFile, IGuestOsStringMapping
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public GuestOsStringMapping()
            : base("Settings\\GuestOsStringMapping.json")
        {
        }
    }
}