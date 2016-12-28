using System.Collections.Generic;

namespace VmMachineHwVersionUpdater.Model
{
    /// <summary>
    /// </summary>
    public interface ILoadHelper
    {
        /// <summary>
        /// </summary>
        string UpdateAllTextBlox { get; set; }

        /// <summary>
        /// </summary>
        List<Machine> VmDataGridItemsSource { get; set; }

        /// <summary>
        /// </summary>
        double? UpdateAllHwVersion { get; set; }

        /// <summary>
        /// </summary>
        List<string> SearchOsItems { get; set; }
    }
}