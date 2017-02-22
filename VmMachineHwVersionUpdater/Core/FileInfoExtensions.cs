using System.IO;

namespace VmMachineHwVersionUpdater.Core
{
    /// <summary>
    /// </summary>
    public static class FileInfoExtensions
    {
        /// <summary>
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        public static string GetProperFilePathCapitalization(this FileInfo fileInfo)
        {
            var dirInfo = fileInfo.Directory;
            if (dirInfo != null)
            {
                return Path.Combine(GetProperDirectoryCapitalization(dirInfo),
                    dirInfo.GetFiles(fileInfo.Name)[0].Name);
            }
            return null;
        }

        /// <summary>
        /// </summary>
        /// <param name="dirInfo"></param>
        /// <returns></returns>
        public static string GetProperDirectoryCapitalization(DirectoryInfo dirInfo)
        {
            var parentDirInfo = dirInfo.Parent;
            if (null == parentDirInfo)
            {
                return dirInfo.Name;
            }
            return Path.Combine(GetProperDirectoryCapitalization(parentDirInfo),
                parentDirInfo.GetDirectories(dirInfo.Name)[0].Name);
        }
    }
}