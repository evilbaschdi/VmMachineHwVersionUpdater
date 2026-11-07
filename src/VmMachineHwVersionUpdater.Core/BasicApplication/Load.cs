using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <inheritdoc cref="ILoad" />
public class Load(IMachinesFromPath machinesFromPath) : ILoad
{
    private readonly IMachinesFromPath _machinesFromPath = machinesFromPath ?? throw new ArgumentNullException(nameof(machinesFromPath));
    private readonly object _cacheLock = new();
    private LoadHelper _cachedValue;
    private bool _hasCachedValue;

    /// <inheritdoc />
    public LoadHelper Value
    {
        get
        {
            lock (_cacheLock)
            {
                if (!_hasCachedValue)
                {
                    _cachedValue = Refresh();
                    _hasCachedValue = true;
                }

                return _cachedValue;
            }
        }
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            lock (_cacheLock)
            {
                _cachedValue = value;
                _hasCachedValue = true;
            }
        }
    }

    /// <inheritdoc />
    public void ResetCache()
    {
        lock (_cacheLock)
        {
            _cachedValue = null;
            _hasCachedValue = false;
        }
    }

    /// <inheritdoc />
    public LoadHelper Refresh()
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
        loadHelper.VmDataGridItemsSource = new ObservableCollection<Machine>(currentItemSource);
        loadHelper.UpdateAllHwVersion = currentItemSource.Select(machine => machine.HwVersion).Max();
        loadHelper.SearchOsItems = searchOsItems;

        return loadHelper;
    }
}