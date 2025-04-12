using System.Collections.ObjectModel;

namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <inheritdoc cref="ILoadSearchOsItems" />
public class LoadSearchOsItems(
    [NotNull] ILoad load,
    [NotNull] IGuestOsesInUse guestOsesInUse,
    [NotNull] ISeparator separator) : CachedWritableValue<ObservableCollection<object>>, ILoadSearchOsItems
{
    private readonly IGuestOsesInUse _guestOsesInUse = guestOsesInUse ?? throw new ArgumentNullException(nameof(guestOsesInUse));
    private readonly ILoad _load = load ?? throw new ArgumentNullException(nameof(load));
    private readonly ISeparator _separator = separator ?? throw new ArgumentNullException(nameof(separator));
    private ObservableCollection<object> _searchOsItemCollection = [];

    /// <inheritdoc />
    protected override ObservableCollection<object> NonCachedValue
    {
        get
        {
            _searchOsItemCollection.Clear();
            _searchOsItemCollection.Add(string.Empty);
            _searchOsItemCollection.Add(_separator.Value);
            foreach (var name in _load.Value.SearchOsItems.OrderBy(item => item.Key))
            {
                _searchOsItemCollection.Add(name.Key);
            }

            _searchOsItemCollection.Add(_separator.Value);

            foreach (var name in _guestOsesInUse.Value.OrderBy(item => item.Key))
            {
                _searchOsItemCollection.Add(name.Key);
            }

            return _searchOsItemCollection;
        }
    }

    /// <inheritdoc />
    protected override void SaveValue(ObservableCollection<object> value)
    {
        _searchOsItemCollection = value ?? throw new ArgumentNullException(nameof(value));
    }
}