using System.Collections;
using System.ComponentModel;
using Avalonia.Collections;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <inheritdoc cref="IConfigureDataGridCollectionView" />
public class ConfigureDataGridCollectionView(
    [NotNull] ILoad load,
    [NotNull] ISettingsValid settingsValid,
    [NotNull] IComparer comparer) : CachedWritableValue<DataGridCollectionView>, IConfigureDataGridCollectionView
{
    private readonly IComparer _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
    private readonly ILoad _load = load ?? throw new ArgumentNullException(nameof(load));
    private readonly ISettingsValid _settingsValid = settingsValid ?? throw new ArgumentNullException(nameof(settingsValid));

    private DataGridCollectionView _dataGridCollectionView;

    /// <inheritdoc />
    protected override DataGridCollectionView NonCachedValue
    {
        get
        {
            var loadValue = _load.Value;
            if (loadValue?.VmDataGridItemsSource == null)
            {
                return new(new List<Machine>());
            }

            if (!_settingsValid.Value)
            {
                //_dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "No virtual machines found", "Please verify settings and discs attached");
                return new(new List<Machine>());
            }

            _dataGridCollectionView = new(loadValue.VmDataGridItemsSource)
                                      {
                                          GroupDescriptions =
                                          {
                                              new DataGridPathGroupDescription("Directory")
                                          }
                                      };

            _dataGridCollectionView.SortDescriptions.Add(new DataGridComparerSortDescription(_comparer, ListSortDirection.Ascending));

            return _dataGridCollectionView;
        }
    }

    /// <inheritdoc />
    protected override void SaveValue(DataGridCollectionView value)
    {
        _dataGridCollectionView = value ?? throw new ArgumentNullException(nameof(value));
    }
}