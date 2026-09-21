using System.Collections.ObjectModel;
using Avalonia.Collections;
using Avalonia.Threading;
using ReactiveUI;
using ReactiveUI.Primitives;
using VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

// ReSharper disable UnusedMember.Global

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <inheritdoc cref="IMainWindowViewModel" />
/// <inheritdoc cref="ViewModelBase" />
public partial class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
{
    private readonly IConfigureDataGridCollectionView _configureDataGridCollectionView;
    private readonly ICurrentMachine _currentMachine;
    private readonly IFilterDataGridCollectionView _filterDataGridCollectionView;
    private readonly IInitReactiveCommands _initReactiveCommands;
    private readonly ILoad _load;
    private readonly ILoadSearchOsItems _loadSearchOsItems;
    private readonly IPathSettings _pathSettings;
    private DispatcherTimer _autoRefreshTimer;

    #region Constructor

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="load"></param>
    /// <param name="currentMachine"></param>
    /// <param name="loadSearchOsItems"></param>
    /// <param name="configureDataGridCollectionView"></param>
    /// <param name="filterDataGridCollectionView"></param>
    /// <param name="initReactiveCommands"></param>
    /// <param name="pathSettings"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public MainWindowViewModel([NotNull] ILoad load,
                               [NotNull] ICurrentMachine currentMachine,
                               [NotNull] ILoadSearchOsItems loadSearchOsItems,
                               [NotNull] IConfigureDataGridCollectionView configureDataGridCollectionView,
                               [NotNull] IFilterDataGridCollectionView filterDataGridCollectionView,
                               [NotNull] IInitReactiveCommands initReactiveCommands,
                               [NotNull] IPathSettings pathSettings)
    {
        _load = load ?? throw new ArgumentNullException(nameof(load));
        _currentMachine = currentMachine ?? throw new ArgumentNullException(nameof(currentMachine));
        _loadSearchOsItems = loadSearchOsItems ?? throw new ArgumentNullException(nameof(loadSearchOsItems));
        _configureDataGridCollectionView = configureDataGridCollectionView ??
                                           throw new ArgumentNullException(nameof(configureDataGridCollectionView));
        _filterDataGridCollectionView = filterDataGridCollectionView ??
                                        throw new ArgumentNullException(nameof(filterDataGridCollectionView));
        _initReactiveCommands = initReactiveCommands ?? throw new ArgumentNullException(nameof(initReactiveCommands));
        _pathSettings = pathSettings ?? throw new ArgumentNullException(nameof(pathSettings));

        Run();
    }

    /// <inheritdoc />
    public void Run()
    {
        AboutWindowCommand = _initReactiveCommands.AboutWindowReactiveCommand.Command;
        AddEditAnnotationCommand = _initReactiveCommands.AddEditAnnotationReactiveCommand.Command;
        ArchiveCommand = _initReactiveCommands.ArchiveReactiveCommand.Command;
        CopyCommand = _initReactiveCommands.CopyReactiveCommand.Command;
        DeleteCommand = _initReactiveCommands.DeleteReactiveCommand.Command;
        GoToCommand = _initReactiveCommands.GoToReactiveCommand.Command;
        OpenWithCodeCommand = _initReactiveCommands.OpenWithCodeReactiveCommand.Command;
        OpenFolderWithCodeCommand = _initReactiveCommands.OpenFolderWithCodeReactiveCommand.Command;
        RenameCommand = _initReactiveCommands.RenameReactiveCommand.Command;
        ReloadCommand = _initReactiveCommands.ReloadReactiveCommand.Command;
        StartCommand = _initReactiveCommands.StartReactiveCommand.Command;
        UpdateAllCommand = _initReactiveCommands.UpdateAllReactiveCommand.Command;

        StartAutoRefresh();
    }

    #endregion Constructor

    /// <summary>
    ///     Binding
    /// </summary>
    public DataGridCollectionView DataGridCollectionViewMachines
    {
        get => _configureDataGridCollectionView.Value;
        set
        {
            _configureDataGridCollectionView.Value = value;
            this.RaisePropertyChanged();
        }
    }

    #region Commands

    /// <summary>
    /// </summary>
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    public ReactiveCommand<RxVoid, RxVoid> AboutWindowCommand { get; set; }

    /// <summary>
    /// </summary>
    public ReactiveCommand<RxVoid, RxVoid> AddEditAnnotationCommand { get; set; }

    /// <summary>
    /// </summary>
    public ReactiveCommand<RxVoid, RxVoid> ArchiveCommand { get; set; }

    /// <summary>
    /// </summary>
    public ReactiveCommand<RxVoid, RxVoid> CopyCommand { get; set; }

    /// <summary>
    /// </summary>
    public ReactiveCommand<RxVoid, RxVoid> DeleteCommand { get; set; }

    /// <summary>
    /// </summary>
    public ReactiveCommand<RxVoid, RxVoid> GoToCommand { get; set; }

    /// <summary>
    /// </summary>
    public ReactiveCommand<RxVoid, RxVoid> OpenWithCodeCommand { get; set; }

    /// <summary>
    /// </summary>
    public ReactiveCommand<RxVoid, RxVoid> OpenFolderWithCodeCommand { get; set; }

    /// <summary>
    /// </summary>
    public ReactiveCommand<RxVoid, RxVoid> ReloadCommand { get; set; }

    /// <summary>
    /// </summary>
    public ReactiveCommand<RxVoid, RxVoid> RenameCommand { get; set; }

    /// <summary>
    /// </summary>
    public ReactiveCommand<RxVoid, RxVoid> StartCommand { get; set; }

    /// <summary>
    /// </summary>
    public ReactiveCommand<RxVoid, RxVoid> UpdateAllCommand { get; set; }
    // ReSharper restore UnusedAutoPropertyAccessor.Global

    /// <summary>
    /// </summary>
    public Machine SelectedMachine
    {
        get => _currentMachine.Value;
        set => _currentMachine.Value = value;
    }

    #endregion Commands

    private void StartAutoRefresh()
    {
        if (_pathSettings.AutoRefreshMinutes <= 0)
        {
            return;
        }

        _autoRefreshTimer = new DispatcherTimer
                            {
                                Interval = TimeSpan.FromMinutes(_pathSettings.AutoRefreshMinutes)
                            };
        _autoRefreshTimer.Tick += (_, _) => RefreshMachineData();
        _autoRefreshTimer.Start();
    }

    private void RefreshMachineData()
    {
        var currentView = _configureDataGridCollectionView.Value;
        currentView.Filter = null!;
        currentView.Refresh();

        _load.ResetCache();
        _loadSearchOsItems.ResetCache();
        _configureDataGridCollectionView.ResetCache();
        DataGridCollectionViewMachines = _configureDataGridCollectionView.Value;
        _filterDataGridCollectionView.RunFor((SearchOsText, SearchFilterText));
        this.RaisePropertyChanged(nameof(SearchOsItemCollection));
        this.RaisePropertyChanged(nameof(SearchOsIsEnabled));
        this.RaisePropertyChanged(nameof(SearchFilterIsReadOnly));
        this.RaisePropertyChanged(nameof(UpdateAllIsEnabled));
        this.RaisePropertyChanged(nameof(UpdateAllTextBlockText));
        this.RaisePropertyChanged(nameof(UpdateAllHwVersionValue));
    }

    #region Properties

    /// <summary>
    ///     Binding
    /// </summary>
    public bool SearchOsIsEnabled =>
        _load.Value?.VmDataGridItemsSource != null && _load.Value.VmDataGridItemsSource.Any();

    /// <summary>
    ///     Binding
    /// </summary>
    public bool SearchFilterIsReadOnly =>
        _load.Value?.VmDataGridItemsSource == null || !_load.Value.VmDataGridItemsSource.Any();

    /// <summary>
    ///     Binding
    /// </summary>
    public ObservableCollection<object> SearchOsItemCollection => _loadSearchOsItems.Value;

    /// <summary>
    ///     Binding
    /// </summary>
    public string SearchFilterText
    {
        get => field;
        set
        {
            this.RaiseAndSetIfChanged(ref field, value);
            _filterDataGridCollectionView.RunFor((SearchOsText, value));
        }
    } = string.Empty;

    /// <summary>
    ///     Binding
    /// </summary>
    public string SearchOsText
    {
        get => field;
        set
        {
            this.RaiseAndSetIfChanged(ref field, value);
            _filterDataGridCollectionView.RunFor((value, SearchFilterText));
        }
    } = string.Empty;

    // ReSharper disable once ValueParameterNotUsed
    /// <summary>
    ///     Binding
    /// </summary>
    public bool UpdateAllIsEnabled =>
        _load.Value?.VmDataGridItemsSource != null && _load.Value.VmDataGridItemsSource.Any();

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