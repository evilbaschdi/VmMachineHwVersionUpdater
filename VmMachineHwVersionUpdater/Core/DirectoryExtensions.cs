using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VmMachineHwVersionUpdater.Core
{
    /// <summary>
    /// </summary>
    public static class DirectoryExtensions
    {
        /// <summary>
        /// </summary>
        /// <param name="directories"></param>
        /// <returns></returns>
        public static List<string> GetExistingDirectories(this List<string> directories)
        {
            var list = new ConcurrentBag<string>();
            Parallel.ForEach(directories, directory =>
                                          {
                                              if (Directory.Exists(directory))
                                              {
                                                  list.Add(directory);
                                              }
                                          });
            return list.ToList();
        }
    }
}