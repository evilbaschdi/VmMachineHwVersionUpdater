using System.Collections.Generic;
using VmMachineHwVersionUpdater.Internal;

namespace VmMachineHwVersionUpdater.Models
{
    /// <summary>
    /// </summary>
    public class LoadHelper
    {
        public string UpdateAllTextBlox { get; set; }

        public List<Machine> VmDataGridItemsSource { get; set; }

        public double? UpdateAllHwVersion { get; set; }

        public List<string> SearchOsItems { get; set; }
    }
}