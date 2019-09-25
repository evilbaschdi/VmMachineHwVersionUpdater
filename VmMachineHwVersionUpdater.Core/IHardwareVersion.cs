using System.Collections.Generic;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.Core
{
    /// <summary>
    /// </summary>
    public interface IHardwareVersion
    {
        /// <summary>
        /// </summary>
        /// <param name="vmxPath"></param>
        /// <param name="newVersion"></param>
        void Update(string vmxPath, int newVersion);

        /// <summary>
        /// </summary>
        /// <param name="vmxPath"></param>
        /// <param name="syncTimeWithHost"></param>
        void EnableSyncTimeWithHost(string vmxPath, bool syncTimeWithHost);

        /// <summary>
        /// </summary>
        /// <param name="vmxPath"></param>
        /// <param name="toolsAutoUpdate"></param>
        void EnableToolsAutoUpdate(string vmxPath, bool toolsAutoUpdate);

        /// <summary>
        /// </summary>
        /// <param name="machinePaths"></param>
        /// <param name="archivePaths"></param>
        /// <returns></returns>
        IEnumerable<Machine> ReadFromPath(List<string> machinePaths, List<string> archivePaths);
    }
}