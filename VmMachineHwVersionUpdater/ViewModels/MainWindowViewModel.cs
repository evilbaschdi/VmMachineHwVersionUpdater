using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Shell;
using EvilBaschdi.CoreExtended;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel;
using VmMachineHwVersionUpdater.Core.BasicApplication;
using VmMachineHwVersionUpdater.Core.Models;
using VmMachineHwVersionUpdater.ViewModels.Internal;

namespace VmMachineHwVersionUpdater.ViewModels;

/// <inheritdoc cref="IMainWindowViewModel" />
/// <inheritdoc cref="INotifyPropertyChanged" />
/// <summary>
///     MainWindowViewModel of VmMachineHwVersionUpdater.
/// </summary>
public class MainWindowViewModel : ApplicationStyleViewModel, IMainWindowViewModel
{
    private readonly IConfigureListCollectionView _configureListCollectionView;
    private readonly ICurrentItem _currentItem;
    private readonly IFilterItemSource _filterItemSource;
    private readonly IInitDefaultCommands _initDefaultCommands;
    private readonly ILoad _load;
    private readonly ILoadSearchOsItems _loadSearchOsItems;
    private readonly ITaskbarItemProgressState _taskbarItemProgressState;
    private string _searchFilterText = string.Empty;
    private string _searchOsText = "(no filter)";

    #region Constructor

    /// <summary>
    ///     Constructor
    /// </summary>
    public MainWindowViewModel(
        IApplicationStyle applicationStyle,
        ICurrentItem currentItem,
        IInitDefaultCommands initDefaultCommands,
        ILoad load,
        IConfigureListCollectionView configureListCollectionView,
        IFilterItemSource filterItemSource,
        ICurrentItemSource currentItemSource,
        ILoadSearchOsItems loadSearchOsItems,
        ITaskbarItemProgressState taskbarItemProgressState
    )
        : base(applicationStyle)
    {
        _currentItem = currentItem ?? throw new ArgumentNullException(nameof(currentItem));
        _initDefaultCommands = initDefaultCommands ?? throw new ArgumentNullException(nameof(initDefaultCommands));
        _load = load ?? throw new ArgumentNullException(nameof(load));
        _configureListCollectionView = configureListCollectionView ?? throw new ArgumentNullException(nameof(configureListCollectionView));
        _filterItemSource = filterItemSource ?? throw new ArgumentNullException(nameof(filterItemSource));
        CurrentItemSource = currentItemSource ?? throw new ArgumentNullException(nameof(currentItemSource));
        _loadSearchOsItems = loadSearchOsItems ?? throw new ArgumentNullException(nameof(loadSearchOsItems));
        _taskbarItemProgressState = taskbarItemProgressState ?? throw new ArgumentNullException(nameof(taskbarItemProgressState));

        Run();
    }

    /// <inheritdoc />
    public void Run()
    {
        _initDefaultCommands.DialogCoordinatorContext = this;
        _initDefaultCommands.Run();

        AboutWindowClick = _initDefaultCommands.AboutWindowClickDefaultCommand.DefaultCommandValue;
        AddEditAnnotation = _initDefaultCommands.AddEditAnnotationDefaultCommand.DefaultCommandValue;
        Archive = _initDefaultCommands.ArchiveDefaultCommand.DefaultCommandValue;
        Copy = _initDefaultCommands.CopyDefaultCommand.DefaultCommandValue;
        Delete = _initDefaultCommands.DeleteDefaultCommand.DefaultCommandValue;
        GoTo = _initDefaultCommands.GotToDefaultCommand.DefaultCommandValue;
        OpenWithCode = _initDefaultCommands.OpenWithCodeDefaultCommand.DefaultCommandValue;
        Reload = _initDefaultCommands.ReloadDefaultCommand.DefaultCommandValue;
        Rename = _initDefaultCommands.RenameDefaultCommand.DefaultCommandValue;
        Start = _initDefaultCommands.StartDefaultCommand.DefaultCommandValue;
        UpdateAll = _initDefaultCommands.UpdateAllDefaultCommand.DefaultCommandValue;
    }

    #endregion Constructor

    #region Commands

    /// <inheritdoc />
    public ICommandViewModel AboutWindowClick { get; set; }

    /// <inheritdoc />
    public ICommandViewModel AddEditAnnotation { get; set; }

    /// <inheritdoc />
    public ICommandViewModel Rename { get; set; }

    /// <inheritdoc />
    public ICommandViewModel Archive { get; set; }

    /// <inheritdoc />
    public ICommandViewModel Copy { get; set; }

    /// <inheritdoc />
    public ICommandViewModel Delete { get; set; }

    /// <inheritdoc />
    public ICommandViewModel GoTo { get; set; }

    /// <inheritdoc />
    public ICommandViewModel OpenWithCode { get; set; }

    /// <inheritdoc />
    public ICurrentItemSource CurrentItemSource { get; }

    /// <inheritdoc />
    public ICommandViewModel Reload { get; set; }

    /// <inheritdoc />
    public ICommandViewModel Start { get; set; }

    /// <inheritdoc />
    public ICommandViewModel UpdateAll { get; set; }

    #endregion Commands

    #region Properties

    /// <summary>
    ///     Binding
    /// </summary>
    public Machine SelectedMachine
    {
        get => _currentItem.Value;
        set
        {
            _currentItem.Value = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Binding
    /// </summary>
    public ListCollectionView ListCollectionView
    {
        get
        {
            _configureListCollectionView.DialogCoordinatorContext = this;
            return _configureListCollectionView.Value;
        }
        set
        {
            _configureListCollectionView.Value = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Binding
    /// </summary>
    public string SearchFilterText
    {
        get => _searchFilterText;
        set
        {
            _searchFilterText = value;
            _filterItemSource.RunFor(SearchOsText, value);
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Binding
    /// </summary>
    public string SearchOsText
    {
        get => _searchOsText;
        set
        {
            _searchOsText = value;
            _filterItemSource.RunFor(value, SearchFilterText);
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Binding
    /// </summary>
    public ObservableCollection<object> SearchOsItemCollection
    {
        get => _loadSearchOsItems.Value;
        set
        {
            _loadSearchOsItems.Value = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Binding
    /// </summary>
    public double? UpdateAllHwVersionValue
    {
        get => _load.Value.UpdateAllHwVersion;
        set
        {
            _load.Value.UpdateAllHwVersion = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Binding for UpdateAllTextBlock
    /// </summary>
    public string UpdateAllTextBlockText
    {
        get => _load.Value.UpdateAllTextBlocks;
        set
        {
            _load.Value.UpdateAllTextBlocks = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Binding
    /// </summary>
    public bool SearchOsIsEnabled
    {
        get =>_load.Value?.VmDataGridItemsSource != null && _load.Value.VmDataGridItemsSource.Any();
        // ReSharper disable once ValueParameterNotUsed
        set => OnPropertyChanged();
    }

    /// <summary>
    ///     Binding
    /// </summary>
    public bool SearchFilterIsReadOnly
    {
        get => _load.Value?.VmDataGridItemsSource == null || !_load.Value.VmDataGridItemsSource.Any();
        // ReSharper disable once ValueParameterNotUsed
        set => OnPropertyChanged();
    }

    /// <summary>
    ///     Binding
    /// </summary>
    public bool UpdateAllIsEnabled
    {
        get => _load.Value?.VmDataGridItemsSource != null && _load.Value.VmDataGridItemsSource.Any();
        // ReSharper disable once ValueParameterNotUsed
        set => OnPropertyChanged();
    }

    /// <summary>
    ///     Binding
    /// </summary>
    public TaskbarItemProgressState ProgressState
    {
        get => _taskbarItemProgressState.Value;

        set
        {
            _taskbarItemProgressState.Value = value;
            OnPropertyChanged();
        }
    }

    #endregion Properties
}