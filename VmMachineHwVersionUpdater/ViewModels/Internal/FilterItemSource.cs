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
            bool filterDisplayNameOrAnnotation;
            var machine = (Machine)vm;

            if (!string.IsNullOrWhiteSpace(searchOsText) && searchOsText != "(no filter)")
            {
                filterGuestOs = machine.GuestOs.StartsWith(searchOsText, StringComparison.InvariantCultureIgnoreCase);
            }

            if (string.IsNullOrWhiteSpace(searchFilterText))
            {
                return filterGuestOs;
            }

            if (searchFilterText.StartsWith('"') && searchFilterText.EndsWith('"') || searchFilterText.StartsWith('\'') && searchFilterText.EndsWith('\''))
            {
                var searchFilterTextTrimmed = searchFilterText.Trim('"', '\'');
                filterDisplayNameOrAnnotation = machine.DisplayName.Contains(searchFilterTextTrimmed, StringComparison.InvariantCultureIgnoreCase)
                                                || machine.Annotation.Contains(searchFilterTextTrimmed, StringComparison.InvariantCultureIgnoreCase);
            }
            else
            {
                var searchFilterTextCharArray = searchFilterText.ToCharArray();
                filterDisplayNameOrAnnotation = searchFilterTextCharArray.All(c => machine.DisplayName.Contains(c, StringComparison.InvariantCultureIgnoreCase)) ||
                                                searchFilterTextCharArray.All(c => machine.Annotation.Contains(c, StringComparison.InvariantCultureIgnoreCase));
            }

            return filterDisplayNameOrAnnotation && filterGuestOs;
        }

        _configureListCollectionView.Value.Filter = ValueFilter;
    }
}