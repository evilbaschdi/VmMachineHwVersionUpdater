using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia.Collections;
using ReactiveUI;
using VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

// ReSharper disable UnusedMember.Global

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <inheritdoc cref="IMainWindowViewModel" />
/// <inheritdoc cref="ViewModelBase" />
public class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
    private readonly IConfigureDataGridCollectionView _configureDataGridCollectionView;
    private readonly ICurrentItem _currentItem;
    private readonly IFilterDataGridCollectionView _filterDataGridCollectionView;
    private readonly IInitReactiveCommands _initReactiveCommands;
    private readonly ILoad _load;
    private readonly ILoadSearchOsItems _loadSearchOsItems;
    private string _searchFilterText = string.Empty;
    private string _searchOsText = string.Empty;

    #region Constructor

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="load"></param>
    /// <param name="currentItem"></param>
    /// <param name="loadSearchOsItems"></param>
    /// <param name="configureDataGridCollectionView"></param>
    /// <param name="filterDataGridCollectionView"></param>
    /// <param name="initReactiveCommands"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public MainWindowViewModel([NotNull] ILoad load,
                               [NotNull] ICurrentItem currentItem,
                               [NotNull] ILoadSearchOsItems loadSearchOsItems,
                               [NotNull] IConfigureDataGridCollectionView configureDataGridCollectionView,
                               [NotNull] IFilterDataGridCollectionView filterDataGridCollectionView,
                               [NotNull] IInitReactiveCommands initReactiveCommands)
    {
        _load = load ?? throw new ArgumentNullException(nameof(load));
        _currentItem = currentItem ?? throw new ArgumentNullException(nameof(currentItem));
        _loadSearchOsItems = loadSearchOsItems ?? throw new ArgumentNullException(nameof(loadSearchOsItems));
        _configureDataGridCollectionView = configureDataGridCollectionView ?? throw new ArgumentNullException(nameof(configureDataGridCollectionView));
        _filterDataGridCollectionView = filterDataGridCollectionView ?? throw new ArgumentNullException(nameof(filterDataGridCollectionView));
        _initReactiveCommands = initReactiveCommands ?? throw new ArgumentNullException(nameof(initReactiveCommands));

        Run();
    }

    /// <inheritdoc />
    public void Run()
    {
        AboutWindowCommand = _initReactiveCommands.AboutWindowReactiveCommand.ReactiveCommandValue;
        AddEditAnnotationCommand = _initReactiveCommands.AddEditAnnotationReactiveCommand.ReactiveCommandValue;
        ArchiveCommand = _initReactiveCommands.ArchiveReactiveCommand.ReactiveCommandValue;
        CopyCommand = _initReactiveCommands.CopyReactiveCommand.ReactiveCommandValue;
        DeleteCommand = _initReactiveCommands.DeleteReactiveCommand.ReactiveCommandValue;
        GoToCommand = _initReactiveCommands.GoToReactiveCommand.ReactiveCommandValue;
        OpenWithCodeCommand = _initReactiveCommands.OpenWithCodeReactiveCommand.ReactiveCommandValue;
        RenameCommand = _initReactiveCommands.RenameReactiveCommand.ReactiveCommandValue;
        ReloadCommand = _initReactiveCommands.ReloadReactiveCommand.ReactiveCommandValue;
        StartCommand = _initReactiveCommands.StartReactiveCommand.ReactiveCommandValue;
        UpdateAllCommand = _initReactiveCommands.UpdateAllReactiveCommand.ReactiveCommandValue;
    }

    #endregion Constructor

    /// <summary>
    ///     Binding
    /// </summary>
    public DataGridCollectionView DataGridCollectionViewMachines
    {
        get => _configureDataGridCollectionView.Value;
        set => _configureDataGridCollectionView.Value = value;
    }

    #region Commands

    /// <summary>
    /// </summary>
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    public ReactiveCommand<Unit, Unit> AboutWindowCommand { get; set; }

    /// <summary>
    /// </summary>
    public ReactiveCommand<Unit, Unit> AddEditAnnotationCommand { get; set; }

    /// <summary>
    /// </summary>
    public ReactiveCommand<Unit, Unit> ArchiveCommand { get; set; }

    /// <summary>
    /// </summary>
    public ReactiveCommand<Unit, Unit> CopyCommand { get; set; }

    /// <summary>
    /// </summary>
    public ReactiveCommand<Unit, Unit> DeleteCommand { get; set; }

    /// <summary>
    /// </summary>
    public ReactiveCommand<Unit, Unit> GoToCommand { get; set; }

    /// <summary>
    /// </summary>
    public ReactiveCommand<Unit, Unit> OpenWithCodeCommand { get; set; }

    /// <summary>
    /// </summary>
    public ReactiveCommand<Unit, Unit> ReloadCommand { get; set; }

    /// <summary>
    /// </summary>
    public ReactiveCommand<Unit, Unit> RenameCommand { get; set; }

    /// <summary>
    /// </summary>
    public ReactiveCommand<Unit, Unit> StartCommand { get; set; }

    /// <summary>
    /// </summary>
    public ReactiveCommand<Unit, Unit> UpdateAllCommand { get; set; }
    // ReSharper restore UnusedAutoPropertyAccessor.Global

    /// <summary>
    /// </summary>
    public Machine SelectedMachine
    {
        get => _currentItem.Value;
        set => _currentItem.Value = value;
    }

    #endregion Commands

    #region Properties

    /// <summary>
    ///     Binding
    /// </summary>
    public bool SearchOsIsEnabled => _load.Value?.VmDataGridItemsSource != null && _load.Value.VmDataGridItemsSource.Any();

    /// <summary>
    ///     Binding
    /// </summary>
    public bool SearchFilterIsReadOnly => _load.Value?.VmDataGridItemsSource == null || !_load.Value.VmDataGridItemsSource.Any();

    /// <summary>
    ///     Binding
    /// </summary>
    public ObservableCollection<object> SearchOsItemCollection => _loadSearchOsItems.Value;

    /// <summary>
    ///     Binding
    /// </summary>
    public string SearchFilterText
    {
        get => _searchFilterText;
        set
        {
            _searchFilterText = value;
            _filterDataGridCollectionView.RunFor((SearchOsText, value));
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
            _filterDataGridCollectionView.RunFor((value, SearchFilterText));
        }
    }

    // ReSharper disable once ValueParameterNotUsed
    /// <summary>
    ///     Binding
    /// </summary>
    public bool UpdateAllIsEnabled => _load.Value?.VmDataGridItemsSource != null && _load.Value.VmDataGridItemsSource.Any();

    /// <summary>
    ///     Binding for UpdateAllTextBlock
    /// </summary>
    public string UpdateAllTextBlockText => _load.Value.UpdateAllTextBlocks;

    /// <summary>
    ///     Binding for UpdateAllHwVersionValue
    /// </summary>
    public double? UpdateAllHwVersionValue
    {
        get => _load.Value.UpdateAllHwVersion;
        set => _load.Value.UpdateAllHwVersion = value;
    }

    // ReSharper disable once ValueParameterNotUsed

    #endregion Properties
}