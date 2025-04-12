using System.Collections.Concurrent;

namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <inheritdoc cref="ILoad" />
public class Load(
    IMachinesFromPath machinesFromPath) : CachedValue<LoadHelper>, ILoad
{
    private readonly IMachinesFromPath _machinesFromPath = machinesFromPath ?? throw new ArgumentNullException(nameof(machinesFromPath));

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

            var searchOsItems = new ConcurrentDictionary<string, bool>();

            Parallel.ForEach(currentItemSource,
                machine =>
                {
                    var guestOs = machine.GuestOs;
                    if (!string.IsNullOrWhiteSpace(guestOs) && !searchOsItems.ContainsKey(guestOs))
                    {
                        searchOsItems.TryAdd(guestOs, true);
                    }
                });

            loadHelper.UpdateAllTextBlocks = $"Update all {currentItemSource.Count} machines to version";
            loadHelper.VmDataGridItemsSource = currentItemSource;
            loadHelper.UpdateAllHwVersion = currentItemSource.Select(machine => machine.HwVersion).Max();
            loadHelper.SearchOsItems = searchOsItems;

            return loadHelper;
        }
    }
}