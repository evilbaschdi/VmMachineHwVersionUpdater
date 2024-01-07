using System.Collections.ObjectModel;

namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <inheritdoc cref="ILoadSearchOsItems" />
/// <summary>
///     Constructor
/// </summary>
/// <param name="load"></param>
/// <param name="guestOsesInUse"></param>
/// <param name="separator"></param>
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
            _load.Value.SearchOsItems?.ForEach(_searchOsItemCollection.Add);
            _searchOsItemCollection.Add(_separator.Value);
            _guestOsesInUse.Value?.ForEach(_searchOsItemCollection.Add);

            return _searchOsItemCollection;
        }
    }

    /// <inheritdoc />
    protected override void SaveValue(ObservableCollection<object> value)
    {
        _searchOsItemCollection = value ?? throw new ArgumentNullException(nameof(value));
    }
}