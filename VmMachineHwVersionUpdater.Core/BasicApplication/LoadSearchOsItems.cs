using System.Collections.ObjectModel;

namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <inheritdoc cref="ILoadSearchOsItems" />
public class LoadSearchOsItems : CachedWritableValue<ObservableCollection<object>>, ILoadSearchOsItems
{
    private readonly IGuestOsesInUse _guestOsesInUse;
    private readonly ILoad _load;
    private readonly ISeparator _separator;
    private ObservableCollection<object> _searchOsItemCollection = new();

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="load"></param>
    /// <param name="guestOsesInUse"></param>
    /// <param name="separator"></param>
    public LoadSearchOsItems([NotNull] ILoad load, [NotNull] IGuestOsesInUse guestOsesInUse, [NotNull] ISeparator separator)
    {
        _load = load ?? throw new ArgumentNullException(nameof(load));
        _guestOsesInUse = guestOsesInUse ?? throw new ArgumentNullException(nameof(guestOsesInUse));
        _separator = separator ?? throw new ArgumentNullException(nameof(separator));
    }

    /// <inheritdoc />
    protected override ObservableCollection<object> NonCachedValue
    {
        get
        {
            _searchOsItemCollection.Clear();
            _load.Value.SearchOsItems?.ForEach(x => _searchOsItemCollection.Add(x));
            //_searchOsItemCollection.Add(new Separator());
            _searchOsItemCollection.Add(_separator.Value);
            _guestOsesInUse.Value?.ForEach(x => _searchOsItemCollection.Add(x));

            return _searchOsItemCollection;
        }
    }

    /// <inheritdoc />
    protected override void SaveValue(ObservableCollection<object> value)
    {
        _searchOsItemCollection = value ?? throw new ArgumentNullException(nameof(value));
    }
}