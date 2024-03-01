namespace VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

/// <inheritdoc />
/// <summary>
///     Constructor
/// </summary>
/// <param name="configureListCollectionView"></param>
/// <param name="filterItemSource"></param>
public class FilterListCollectionView(
    [NotNull] IConfigureListCollectionView configureListCollectionView,
    [NotNull] IFilterItemSource filterItemSource) : IFilterListCollectionView
{
    private readonly IConfigureListCollectionView
        _configureListCollectionView = configureListCollectionView ?? throw new ArgumentNullException(nameof(configureListCollectionView));

    private readonly IFilterItemSource _filterItemSource = filterItemSource ?? throw new ArgumentNullException(nameof(filterItemSource));

    /// <inheritdoc />
    public void RunFor((string SearchOsText, string SearchFilterText) value)
    {
        var (searchOsText, searchFilterText) = value;

        bool ValueFilter(object vm)
        {
            var machine = (Machine)vm;
            return _filterItemSource.ValueFor((machine, searchOsText, searchFilterText));
        }

        _configureListCollectionView.Value.Filter = ValueFilter;
    }
}