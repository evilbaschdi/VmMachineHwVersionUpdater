using VmMachineHwVersionUpdater.Core.BasicApplication;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.ViewModels.Internal;

/// <inheritdoc />
public class FilterListCollectionView : IFilterListCollectionView
{
    private readonly IConfigureListCollectionView _configureListCollectionView;
    private readonly IFilterItemSource _filterItemSource;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="configureListCollectionView"></param>
    /// <param name="filterItemSource"></param>
    public FilterListCollectionView([NotNull] IConfigureListCollectionView configureListCollectionView,
                                    [NotNull] IFilterItemSource filterItemSource)
    {
        _configureListCollectionView = configureListCollectionView ?? throw new ArgumentNullException(nameof(configureListCollectionView));
        _filterItemSource = filterItemSource ?? throw new ArgumentNullException(nameof(filterItemSource));
    }

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