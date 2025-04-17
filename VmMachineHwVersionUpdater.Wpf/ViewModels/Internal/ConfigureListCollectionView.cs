using System.ComponentModel;
using System.Windows.Data;

namespace VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

/// <inheritdoc cref="IConfigureListCollectionView" />
public class ConfigureListCollectionView(
    [NotNull] ILoad load) : CachedWritableValue<ListCollectionView>, IConfigureListCollectionView
{
    private readonly ILoad _load = load ?? throw new ArgumentNullException(nameof(load));
    private ListCollectionView _listCollectionView;

    /// <inheritdoc />
    protected override ListCollectionView NonCachedValue
    {
        get
        {
            var loadValue = _load.Value;

            //_dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "Verifying VM pools from settings", $"{loadValue.VmDataGridItemsSource.Count} paths found");

            _listCollectionView = new(loadValue.VmDataGridItemsSource.ToList());
            _listCollectionView?.GroupDescriptions?.Add(new PropertyGroupDescription("Directory"));
            _listCollectionView?.SortDescriptions.Add(new("DisplayName", ListSortDirection.Ascending));

            return _listCollectionView;
        }
    }

    /// <inheritdoc />
    public object DialogCoordinatorContext { get; set; }

    /// <inheritdoc />
    protected override void SaveValue(ListCollectionView value)
    {
        _listCollectionView = value ?? throw new ArgumentNullException(nameof(value));
    }
}