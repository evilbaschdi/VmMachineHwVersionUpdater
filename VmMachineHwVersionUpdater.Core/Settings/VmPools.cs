using System.IO;
using Microsoft.Extensions.Configuration;

namespace VmMachineHwVersionUpdater.Core.Settings
{
    /// <summary>
    /// </summary>
    public static class VmPools
    {
        static VmPools()
        {
            AppSetting = new ConfigurationBuilder()
                         .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile("Settings\\VmPools.json")
                         .Build();
        }

        /// <summary>
        /// </summary>
        public static IConfiguration AppSetting { get; }
    }
}