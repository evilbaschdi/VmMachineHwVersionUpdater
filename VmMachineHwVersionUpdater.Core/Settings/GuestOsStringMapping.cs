using System.IO;
using Microsoft.Extensions.Configuration;

namespace VmMachineHwVersionUpdater.Core.Settings
{
    /// <summary>
    /// </summary>
    public static class GuestOsStringMapping
    {
        static GuestOsStringMapping()
        {
            AppSetting = new ConfigurationBuilder()
                         .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile("Settings\\GuestOsStringMapping.json")
                         .Build();
        }


        /// <summary>
        /// </summary>
        public static IConfiguration AppSetting { get; }
    }
}