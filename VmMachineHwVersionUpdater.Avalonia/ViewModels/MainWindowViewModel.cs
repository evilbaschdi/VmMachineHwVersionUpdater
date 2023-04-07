using System.Collections;
using System.ComponentModel;
using Avalonia.Collections;
using VmMachineHwVersionUpdater.Core.BasicApplication;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <inheritdoc />
public class MainWindowViewModel : ViewModelBase
{
    private readonly ILoad _load;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="load"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public MainWindowViewModel(ILoad load)
    {
        _load = load ?? throw new ArgumentNullException(nameof(load));
    }

    /// <summary>
    ///     Binding
    /// </summary>
    public DataGridCollectionView DataGridCollectionViewMachines
    {
        get
        {
            if (_load.Value.VmDataGridItemsSource != null)
            {
                var dataGridCollectionView = new DataGridCollectionView(_load.Value.VmDataGridItemsSource)
                                             {
                                                 GroupDescriptions =
                                                 {
                                                     new DataGridPathGroupDescription("Directory")
                                                 }
                                             };

                IComparer comparer = new MachineComparer();
                dataGridCollectionView.SortDescriptions.Add(new DataGridComparerSortDescription(comparer, ListSortDirection.Ascending));

                return dataGridCollectionView;
            }

            return new DataGridCollectionView(new List<Machine>());
        }
        // ReSharper disable once ValueParameterNotUsed
        set => _load.Value.VmDataGridItemsSource = new List<Machine>();
    }
}