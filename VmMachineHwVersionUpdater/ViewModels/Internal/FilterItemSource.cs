using JetBrains.Annotations;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.ViewModels.Internal;

/// <inheritdoc />
public class FilterItemSource : IFilterItemSource
{
    private readonly IConfigureListCollectionView _configureListCollectionView;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="configureListCollectionView"></param>
    public FilterItemSource([NotNull] IConfigureListCollectionView configureListCollectionView)
    {
        _configureListCollectionView = configureListCollectionView ?? throw new ArgumentNullException(nameof(configureListCollectionView));
    }

    /// <inheritdoc />
    public void RunFor(string searchOsText, string searchFilterText)
    {
        if (searchOsText is null)
        {
            throw new ArgumentNullException(nameof(searchOsText));
        }

        if (searchFilterText is null)
        {
            throw new ArgumentNullException(nameof(searchFilterText));
        }

        bool ValueFilter(object vm)
        {
            var filterGuestOs = true;
            var filterDisplayNameOrAnnotation = true;
            var machine = (Machine)vm;

            if (!string.IsNullOrWhiteSpace(searchOsText) && searchOsText != "(no filter)")
            {
                filterGuestOs = machine.GuestOs.StartsWith(searchOsText, StringComparison.InvariantCultureIgnoreCase);
            }

            if (!string.IsNullOrWhiteSpace(searchFilterText))
            {
                filterDisplayNameOrAnnotation = machine.DisplayName.Contains(searchFilterText, StringComparison.InvariantCultureIgnoreCase)
                                                || machine.Annotation.Contains(searchFilterText, StringComparison.InvariantCultureIgnoreCase);
            }

            return filterDisplayNameOrAnnotation && filterGuestOs;
        }

        _configureListCollectionView.Value.Filter = ValueFilter;
    }
}