using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Shell;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using VmMachineHwVersionUpdater.Core.Models;
using VmMachineHwVersionUpdater.ViewModels.Internal;

namespace VmMachineHwVersionUpdater.ViewModels;

/// <inheritdoc />
public interface IMainWindowViewModel : IRun
{
    /// <summary />
    // ReSharper disable UnusedMemberInSuper.Global
    // ReSharper disable UnusedMember.Global
    public ICommandViewModel AboutWindowClick { get; set; }

    /// <summary />
    public ICommandViewModel AddEditAnnotation { get; set; }

    /// <summary />
    public ICommandViewModel Rename { get; set; }

    /// <summary />
    public ICommandViewModel Archive { get; set; }

    /// <summary>
    /// </summary>
    public ICommandViewModel Copy { get; set; }

    /// <summary />
    public ICommandViewModel Delete { get; set; }

    /// <summary />
    public ICommandViewModel GoTo { get; set; }

    /// <summary />
    public ICommandViewModel OpenWithCode { get; set; }

    /// <summary />
    public ICurrentItemSource CurrentItemSource { get; }

    /// <summary />
    public ICommandViewModel Reload { get; set; }

    /// <summary />
    public ICommandViewModel Start { get; set; }

    /// <summary />
    public ICommandViewModel UpdateAll { get; set; }

    /// <summary>
    ///     Binding
    /// </summary>

    public Machine SelectedMachine { get; set; }

    /// <summary>
    ///     Binding
    /// </summary>
    public ListCollectionView ListCollectionView { get; set; }

    /// <summary>
    ///     Binding
    /// </summary>
    public string SearchFilterText { get; set; }

    /// <summary>
    ///     Binding
    /// </summary>
    public string SearchOsText { get; set; }

    /// <summary>
    ///     Binding
    /// </summary>
    public ObservableCollection<object> SearchOsItemCollection { get; set; }

    /// <summary>
    ///     Binding
    /// </summary>
    public double? UpdateAllHwVersionValue { get; set; }

    /// <summary>
    ///     Binding for UpdateAllTextBlock
    /// </summary>
    public string UpdateAllTextBlockText { get; set; }

    /// <summary>
    ///     Binding
    /// </summary>
    public bool SearchOsIsEnabled { get; set; }

    /// <summary>
    ///     Binding
    /// </summary>
    public bool SearchFilterIsReadOnly { get; set; }

    /// <summary>
    ///     Binding
    /// </summary>
    public bool UpdateAllIsEnabled { get; set; }

    /// <summary>
    ///     Binding
    /// </summary>
    public TaskbarItemProgressState ProgressState { get; set; }
    // ReSharper restore UnusedMember.Global
    // ReSharper restore UnusedMemberInSuper.Global
}