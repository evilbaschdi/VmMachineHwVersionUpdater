using System.IO;
using System.Linq;

namespace VmMachineHwVersionUpdater.Extensions
{
    /// <summary>
    /// </summary>
    public static class DirectoryExtensions
    {
        /// <summary>
        /// </summary>
        public static double GetDirectorySize(this DirectoryInfo dir)
        {
            var sum = dir.GetFiles().Aggregate<FileInfo, double>(0, (current, file) => current + file.Length);
            return dir.GetDirectories().Aggregate(sum, (current, dir1) => current + GetDirectorySize(dir1));
        }
    }
}