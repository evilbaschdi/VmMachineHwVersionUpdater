﻿using System;
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

        /// <summary>
        /// </summary>
        public static double GetDirectorySize(this DirectoryInfo dir)
        {
            var sum = dir.GetFiles().Aggregate<FileInfo, double>(0, (current, file) => current + file.Length);
            return dir.GetDirectories().Aggregate(sum, (current, dir1) => current + GetDirectorySize(dir1));
        }
    }
}