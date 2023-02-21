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
            var dataGridCollectionView = new DataGridCollectionView(_load.Value.VmDataGridItemsSource)
                                         {
                                             GroupDescriptions =
                                             {
                                                 new DataGridPathGroupDescription("Directory")
                                             }
                                         };

            //IComparer comparer = new Comparer(CultureInfo.InvariantCulture);
            //dataGridCollectionView.SortDescriptions.Add(new DataGridComparerSortDescription(comparer, ListSortDirection.Ascending));

            return dataGridCollectionView;
        }
        // ReSharper disable once ValueParameterNotUsed
        set => _load.Value.VmDataGridItemsSource = new List<Machine>();
    }
}