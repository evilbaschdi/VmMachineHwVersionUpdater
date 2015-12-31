using System.Collections.Generic;

namespace VmMachineHwVersionUpdater.Internal
{
    /// <summary>
    /// </summary>
    public interface IFilePath
    {
        /// <summary>
        /// </summary>
        /// <param name="initialDirectory"></param>
        /// <returns></returns>
        List<string> GetFileList(string initialDirectory);
    }
}