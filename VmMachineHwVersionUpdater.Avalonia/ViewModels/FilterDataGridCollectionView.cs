namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <inheritdoc />
public class FilterDataGridCollectionView(
    [NotNull] IConfigureDataGridCollectionView configureDataGridCollectionView,
    [NotNull] IFilterItemSource filterItemSource) : IFilterDataGridCollectionView
{
    private readonly IConfigureDataGridCollectionView _configureDataGridCollectionView =
        configureDataGridCollectionView ?? throw new ArgumentNullException(nameof(configureDataGridCollectionView));

    private readonly IFilterItemSource _filterItemSource = filterItemSource ?? throw new ArgumentNullException(nameof(filterItemSource));

    /// <inheritdoc />
    public void RunFor((string SearchOsText, string SearchFilterText) value)
    {
        var (searchOsText, searchFilterText) = value;

        _configureDataGridCollectionView.Value.Filter = ValueFilter;
        return;

        bool ValueFilter(object vm)
        {
            var machine = (Machine)vm;
            return _filterItemSource.ValueFor((machine, searchOsText, searchFilterText));
        }
    }
}