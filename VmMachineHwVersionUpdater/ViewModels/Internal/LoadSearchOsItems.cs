using System.Collections.ObjectModel;
using System.Windows.Controls;
using EvilBaschdi.Core;
using JetBrains.Annotations;
using VmMachineHwVersionUpdater.Core.BasicApplication;

namespace VmMachineHwVersionUpdater.ViewModels.Internal;

/// <inheritdoc cref="ILoadSearchOsItems" />
public class LoadSearchOsItems : CachedWritableValue<ObservableCollection<object>>, ILoadSearchOsItems
{
    private readonly IGuestOsesInUse _guestOsesInUse;
    private readonly ILoad _load;
    private ObservableCollection<object> _searchOsItemCollection = new();

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="load"></param>
    /// <param name="guestOsesInUse"></param>
    public LoadSearchOsItems([NotNull] ILoad load, [NotNull] IGuestOsesInUse guestOsesInUse)
    {
        _load = load ?? throw new ArgumentNullException(nameof(load));
        _guestOsesInUse = guestOsesInUse ?? throw new ArgumentNullException(nameof(guestOsesInUse));
    }

    /// <inheritdoc />
    protected override ObservableCollection<object> NonCachedValue
    {
        get
        {
            _searchOsItemCollection.Clear();
            _searchOsItemCollection.Add("(no filter)");
            _searchOsItemCollection.Add(new Separator());
            _load.Value.SearchOsItems?.ForEach(x => _searchOsItemCollection.Add(x));
            _searchOsItemCollection.Add(new Separator());
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