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
    private readonly ICurrentMachine _currentMachine;
    private readonly IFilterDataGridCollectionView _filterDataGridCollectionView;
    private readonly IInitReactiveCommands _initReactiveCommands;
    private readonly ILoad _load;
    private readonly ILoadSearchOsItems _loadSearchOsItems;

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
    /// <exception cref="ArgumentNullException"></exception>
    public MainWindowViewModel([NotNull] ILoad load,
                               [NotNull] ICurrentMachine currentMachine,
                               [NotNull] ILoadSearchOsItems loadSearchOsItems,
                               [NotNull] IConfigureDataGridCollectionView configureDataGridCollectionView,
                               [NotNull] IFilterDataGridCollectionView filterDataGridCollectionView,
                               [NotNull] IInitReactiveCommands initReactiveCommands)
    {
        _load = load ?? throw new ArgumentNullException(nameof(load));
        _currentMachine = currentMachine ?? throw new ArgumentNullException(nameof(currentMachine));
        _loadSearchOsItems = loadSearchOsItems ?? throw new ArgumentNullException(nameof(loadSearchOsItems));
        _configureDataGridCollectionView = configureDataGridCollectionView ?? throw new ArgumentNullException(nameof(configureDataGridCollectionView));
        _filterDataGridCollectionView = filterDataGridCollectionView ?? throw new ArgumentNullException(nameof(filterDataGridCollectionView));
        _initReactiveCommands = initReactiveCommands ?? throw new ArgumentNullException(nameof(initReactiveCommands));

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
        RenameCommand = _initReactiveCommands.RenameReactiveCommand.Command;
        ReloadCommand = _initReactiveCommands.ReloadReactiveCommand.Command;
        StartCommand = _initReactiveCommands.StartReactiveCommand.Command;
        UpdateAllCommand = _initReactiveCommands.UpdateAllReactiveCommand.Command;
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
        get => _currentMachine.Value;
        set => _currentMachine.Value = value;
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
    private string SearchFilterText
    {
        get;
        // ReSharper disable once PropertyCanBeMadeInitOnly.Local
        // ReSharper disable once UnusedMember.Local
        set
        {
            field = value;
            _filterDataGridCollectionView.RunFor((SearchOsText, value));
        }
    } = string.Empty;

    /// <summary>
    ///     Binding
    /// </summary>
    private string SearchOsText
    {
        get;
        // ReSharper disable once PropertyCanBeMadeInitOnly.Local
        // ReSharper disable once UnusedMember.Local
        set
        {
            field = value;
            _filterDataGridCollectionView.RunFor((value, SearchFilterText));
        }
    } = string.Empty;

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