using System.Collections.Generic;

namespace VmMachineHwVersionUpdater.Model
{
    /// <inheritdoc />
    public class LoadHelper : ILoadHelper
    {
        /// <inheritdoc />

        public string UpdateAllTextBlox { get; set; }

        /// <inheritdoc />
        public List<Machine> VmDataGridItemsSource { get; set; }

        /// <inheritdoc />
        public double? UpdateAllHwVersion { get; set; }

        /// <inheritdoc />
        public List<string> SearchOsItems { get; set; }
    }
}