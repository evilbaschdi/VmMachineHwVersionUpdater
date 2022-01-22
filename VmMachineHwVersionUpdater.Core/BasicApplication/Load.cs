using System;
using System.Collections.Generic;
using System.Linq;
using EvilBaschdi.Core;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <inheritdoc cref="ILoad" />
public class Load : CachedValue<LoadHelper>, ILoad
{
    private readonly IMachinesFromPath _machinesFromPath;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="machinesFromPath"></param>
    public Load(IMachinesFromPath machinesFromPath)
    {
        _machinesFromPath = machinesFromPath ?? throw new ArgumentNullException(nameof(machinesFromPath));
    }

    /// <inheritdoc />
    protected override LoadHelper NonCachedValue
    {
        get
        {
            var loadHelper = new LoadHelper();

            var currentItemSource = _machinesFromPath.Value;

            if (!currentItemSource.Any())
            {
                return loadHelper;
            }

            var searchOsItems = new List<string>();
            foreach (var machine in currentItemSource.OrderBy(m => m.GuestOs)
                                                     .Where(item => !searchOsItems.Contains(item.GuestOs)))
            {
                searchOsItems.Add(machine.GuestOs);
            }

            loadHelper.UpdateAllTextBlocks = $"Update all {currentItemSource.Count} machines to version";
            loadHelper.VmDataGridItemsSource = currentItemSource;
            loadHelper.UpdateAllHwVersion = currentItemSource.Select(machine => machine.HwVersion).Max();
            loadHelper.SearchOsItems = searchOsItems;

            return loadHelper;
        }
    }
}