﻿using System.Collections.Generic;

namespace VmMachineHwVersionUpdater.Model
{
    /// <summary />
    public class LoadHelper
    {
        /// <summary />
        public List<string> SearchOsItems { get; set; }

        /// <summary />
        public double? UpdateAllHwVersion { get; set; }

        /// <summary />

        public string UpdateAllTextBlocks { get; set; }

        /// <summary />
        public List<Machine> VmDataGridItemsSource { get; set; }
    }
}