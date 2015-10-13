using System.Collections.Generic;

namespace VmMachineHwVersionUpdater.Internal
{
    public interface IFilePath
    {
        List<string> GetFileList(string initialDirectory);
    }
}