namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <inheritdoc />
public interface IFilterItemSource : IValueFor<(Machine Machine, string SearchOsText, string SearchFilterText), bool>
{
}