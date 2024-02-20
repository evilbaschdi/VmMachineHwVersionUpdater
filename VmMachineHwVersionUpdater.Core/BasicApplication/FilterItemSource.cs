namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <inheritdoc />
public class FilterItemSource : IFilterItemSource
{
    /// <inheritdoc />
    public bool ValueFor((Machine Machine, string SearchOsText, string SearchFilterText) value)
    {
        var (machine, searchOsText, searchFilterText) = value;

        var filterGuestOs = true;
        bool filterDisplayNameOrAnnotation;

        if (!string.IsNullOrWhiteSpace(searchOsText) && searchOsText != "(no filter)")
        {
            filterGuestOs = machine.GuestOs.StartsWith(searchOsText, StringComparison.OrdinalIgnoreCase);
        }

        if (string.IsNullOrWhiteSpace(searchFilterText))
        {
            return filterGuestOs;
        }

        if (searchFilterText.StartsWith('*') && searchFilterText.EndsWith('*'))
        {
            var searchFilterTextTrimmed = searchFilterText.Trim('*', '\'');
            var searchFilterTextCharArray = searchFilterTextTrimmed.ToCharArray();
            filterDisplayNameOrAnnotation = searchFilterTextCharArray.All(c => machine.DisplayName.Contains(c, StringComparison.OrdinalIgnoreCase)) ||
                                            searchFilterTextCharArray.All(c => machine.Annotation.Contains(c, StringComparison.OrdinalIgnoreCase));
        }
        else
        {
            filterDisplayNameOrAnnotation = machine.DisplayName.Contains(searchFilterText, StringComparison.OrdinalIgnoreCase)
                                            || machine.Annotation.Contains(searchFilterText, StringComparison.OrdinalIgnoreCase);
        }

        return filterDisplayNameOrAnnotation && filterGuestOs;
    }
}