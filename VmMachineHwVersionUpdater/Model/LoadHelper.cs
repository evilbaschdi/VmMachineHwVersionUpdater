using System.Collections.Generic;

namespace VmMachineHwVersionUpdater.Model
{
    /// <summary>
    /// </summary>
    public class LoadHelper : ILoadHelper
    {
        /// <summary>
        /// </summary>
        public string UpdateAllTextBlox { get; set; }

        /// <summary>
        /// </summary>
        public List<Machine> VmDataGridItemsSource { get; set; }

        /// <summary>
        /// </summary>
        public double? UpdateAllHwVersion { get; set; }

        /// <summary>
        /// </summary>
        public List<string> SearchOsItems { get; set; }
    }
}