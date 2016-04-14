using System.Collections.Generic;

namespace VmMachineHwVersionUpdater.Internal
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
        /// <param name="machinePath"></param>
        /// <returns></returns>
        IEnumerable<Machine> ReadFromPath(string machinePath);
    }
}