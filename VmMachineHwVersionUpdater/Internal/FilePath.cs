using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VmMachineHwVersionUpdater.Extensions;

namespace VmMachineHwVersionUpdater.Internal
{
    public class FilePath : IFilePath
    {
        private IEnumerable<string> GetSubdirectoriesContainingOnlyFiles(string path)
        {
            return Directory.GetDirectories(path, "*", SearchOption.AllDirectories).ToList();
        }

        public List<string> GetFileList(string initialDirectory)
        {
            var fileList = new List<string>();
            var initialDirectoryFileList = Directory.GetFiles(initialDirectory).ToList();
            Parallel.ForEach(initialDirectoryFileList.Where(file => IsValidFileName(file, fileList)),
                file => fileList.Add(file));

            var initialDirectorySubdirectoriesFileList =
                GetSubdirectoriesContainingOnlyFiles(initialDirectory)
                    .SelectMany(Directory.GetFiles)
                    .Where(IsValidFileName);

            Parallel.ForEach(initialDirectorySubdirectoriesFileList.Where(file => IsValidFileName(file, fileList)),
                file => fileList.Add(file));

            return fileList;
        }

        private bool IsValidFileName(string file)
        {
            const string type = "vmx";
            var fileExtension = Path.GetExtension(file);

            var directoryInfo = new FileInfo(file).Directory;

            if(directoryInfo == null)
            {
                return false;
            }
            var dir = directoryInfo.FullName;
            return dir.IsAccessible() && !string.IsNullOrWhiteSpace(fileExtension) &&
                   fileExtension.ToLower().EndsWith(type);
        }

        private bool IsValidFileName(string file, ICollection<string> fileList)
        {
            const string type = "vmx";
            var fileExtension = Path.GetExtension(file);

            var directoryInfo = new FileInfo(file).Directory;

            if(directoryInfo == null)
            {
                return false;
            }
            var dir = directoryInfo.FullName;
            return dir.IsAccessible() && !fileList.Contains(file) && !string.IsNullOrWhiteSpace(fileExtension) &&
                   fileExtension.ToLower().EndsWith(type);
        }
    }
}