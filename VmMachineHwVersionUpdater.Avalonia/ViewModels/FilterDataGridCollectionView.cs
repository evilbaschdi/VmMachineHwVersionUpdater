namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <inheritdoc />
/// <summary>
///     Constructor
/// </summary>
/// <param name="configureDataGridCollectionView"></param>
/// <param name="filterItemSource"></param>
/// <exception cref="ArgumentNullException"></exception>
public class FilterDataGridCollectionView([NotNull] IConfigureDataGridCollectionView configureDataGridCollectionView,
                                    [NotNull] IFilterItemSource filterItemSource) : IFilterDataGridCollectionView
{
    private readonly IConfigureDataGridCollectionView _configureDataGridCollectionView = configureDataGridCollectionView ?? throw new ArgumentNullException(nameof(configureDataGridCollectionView));
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

        _configureDataGridCollectionView.Value.Filter = ValueFilter;
    }
}