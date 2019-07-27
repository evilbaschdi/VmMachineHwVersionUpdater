using System.IO;
using Microsoft.Extensions.Configuration;

namespace VmMachineHwVersionUpdater.Core
{
    /// <summary>
    /// </summary>
    public static class GuestOsStringMapping
    {
        static GuestOsStringMapping()
        {
            AppSetting = new ConfigurationBuilder()
                         .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile("GuestOsStringMapping.json")
                         .Build();
        }


        /// <summary>
        /// </summary>
        public static IConfiguration AppSetting { get; }
    }
}