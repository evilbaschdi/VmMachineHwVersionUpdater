using System;
using System.IO;

namespace VmMachineHwVersionUpdater.Extensions
{
    public static class DirectoryExtensions
    {
        public static bool IsAccessible(this string path)
        {
            //get directory info
            var realpath = new DirectoryInfo(path);
            try
            {
                //if GetDirectories works then is accessible
                realpath.GetDirectories();
                return true;
            }
            catch(Exception)
            {
                //if exception is not accesible
                return false;
            }
        }
    }
}