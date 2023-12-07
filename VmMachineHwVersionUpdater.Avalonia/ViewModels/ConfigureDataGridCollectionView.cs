using System.Collections;
using System.ComponentModel;
using Avalonia.Collections;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <inheritdoc cref="IConfigureDataGridCollectionView" />
/// <summary>
///     Constructor
/// </summary>
/// <param name="load"></param>
/// <param name="settingsValid"></param>
/// <param name="comparer"></param>
/// <exception cref="ArgumentNullException"></exception>
public class ConfigureDataGridCollectionView([NotNull] ILoad load,
                                       [NotNull] ISettingsValid settingsValid,
                                       [NotNull] IComparer comparer) : CachedWritableValue<DataGridCollectionView>, IConfigureDataGridCollectionView
{
    private readonly IComparer _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
    [NotNull] private readonly ILoad _load = load ?? throw new ArgumentNullException(nameof(load));
    [NotNull] private readonly ISettingsValid _settingsValid = settingsValid ?? throw new ArgumentNullException(nameof(settingsValid));

    private DataGridCollectionView _dataGridCollectionView;

    /// <inheritdoc />
    protected override DataGridCollectionView NonCachedValue
    {
        get
        {
            var loadValue = _load.Value;
            if (loadValue?.VmDataGridItemsSource == null)
            {
                return new DataGridCollectionView(new List<Machine>());
            }

            if (!_settingsValid.Value)
            {
                //_dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "No virtual machines found", "Please verify settings and discs attached");
                return new(new List<Machine>());
            }

            _dataGridCollectionView = new DataGridCollectionView(loadValue.VmDataGridItemsSource)
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