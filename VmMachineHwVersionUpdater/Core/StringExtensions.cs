using System;
using System.Globalization;

namespace VmMachineHwVersionUpdater.Core
{
    /// <summary>
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///     Converts a double into FileSize string
        /// </summary>
        /// <param name="d"></param>
        /// <param name="precision"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static string ToFileSize(this double d, int precision, CultureInfo culture)
        {
            var size = Convert.ToUInt64(d);

            if (size < 1024d)
            {
                return $"{size.ToString("F0", culture)} bytes";
            }

            if (size >> 10 < 1024d)
            {
                return (size / (float) 1024d).ToString($"F{precision}", culture) + " KB";
            }

            if (size >> 20 < 1024d)
            {
                return $"{((size >> 10) / (float) 1024d).ToString($"F{precision}", culture)} MB";
            }

            if (size >> 30 < 1024d)
            {
                return $"{((size >> 20) / (float) 1024d).ToString($"F{precision}", culture)} GB";
            }

            if (size >> 40 < 1024d)
            {
                var workaround = size / 1000 * 1024;
                return $"{((workaround >> 30) / (float) 1024d).ToString($"F{precision}", culture)} TB";
            }

            if (size >> 50 < 1024d)
            {
                return $"{((size >> 40) / (float) 1024d).ToString($"F{precision}", culture)} PB";
            }

            return $"{((size >> 50) / (float) 1024d).ToString($"F{precision}", culture)} EB";
        }
    }
}