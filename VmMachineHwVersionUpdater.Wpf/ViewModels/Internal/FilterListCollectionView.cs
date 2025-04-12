namespace VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

/// <inheritdoc />
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

        _configureListCollectionView.Value.Filter = ValueFilter;
        return;

        bool ValueFilter(object vm)
        {
            var machine = (Machine)vm;
            return _filterItemSource.ValueFor((machine, searchOsText, searchFilterText));
        }
    }
}