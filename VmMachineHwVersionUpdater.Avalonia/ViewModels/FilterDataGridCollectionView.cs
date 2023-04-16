using JetBrains.Annotations;
using VmMachineHwVersionUpdater.Core.BasicApplication;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <inheritdoc />
public class FilterDataGridCollectionView : IFilterDataGridCollectionView
{
    private readonly IConfigureDataGridCollectionView _configureDataGridCollectionView;
    private readonly IFilterItemSource _filterItemSource;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="configureDataGridCollectionView"></param>
    /// <param name="filterItemSource"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public FilterDataGridCollectionView([NotNull] IConfigureDataGridCollectionView configureDataGridCollectionView,
                                        [NotNull] IFilterItemSource filterItemSource)
    {
        _configureDataGridCollectionView = configureDataGridCollectionView ?? throw new ArgumentNullException(nameof(configureDataGridCollectionView));
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

        _configureDataGridCollectionView.Value.Filter = ValueFilter;
    }
}