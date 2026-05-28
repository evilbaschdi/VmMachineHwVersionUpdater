using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace VmMachineHwVersionUpdater.Core.Models;

/// <summary />
public class LoadHelper
{
    /// <summary />
    public ConcurrentDictionary<string, bool> SearchOsItems { get; set; }

    /// <summary />
    public double? UpdateAllHwVersion { get; set; }

    /// <summary />
    public string UpdateAllTextBlocks { get; set; }

    /// <summary />
    public ObservableCollection<Machine> VmDataGridItemsSource { get; set; }
}