using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Avalonia.Collections;
using Avalonia.Threading;
using ReactiveUI;
using ReactiveUI.Primitives;
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
    private readonly SemaphoreSlim _refreshSemaphore = new(1, 1);
    private readonly IPathSettings _pathSettings;
    private readonly StringComparer _pathComparer = OperatingSystem.IsWindows()
                                                        ? StringComparer.OrdinalIgnoreCase
                                                        : StringComparer.Ordinal;
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
        if (_pathSettings.AutoRefreshMinutes <= 0 || _autoRefreshTimer != null)
        {
            return;
        }

        _autoRefreshTimer = new DispatcherTimer
                            {
                                Interval = TimeSpan.FromMinutes(_pathSettings.AutoRefreshMinutes)
                            };
        _autoRefreshTimer.Tick += OnAutoRefreshTimerTick;
        _autoRefreshTimer.Start();
    }

    private async void OnAutoRefreshTimerTick(object sender, EventArgs e)
    {
        await RefreshMachineDataAsync();
    }

    /// <inheritdoc />
    public async Task RefreshMachineDataAsync()
    {
        if (!await _refreshSemaphore.WaitAsync(0))
        {
            return;
        }

        try
        {
            var refreshedLoad = await Task.Run(_load.Refresh);
            ApplyRefreshedLoad(refreshedLoad);
        }
        catch (Exception exception)
        {
            Trace.TraceError("Automatic VM refresh failed. The current UI data was kept. {0}", exception);
        }
        finally
        {
            _refreshSemaphore.Release();
        }
    }

    private void ApplyRefreshedLoad(LoadHelper refreshedLoad)
    {
        ArgumentNullException.ThrowIfNull(refreshedLoad);

        var selectedMachinePath = SelectedMachine?.Path;
        var searchOsText = SearchOsText;
        var searchFilterText = SearchFilterText;
        var currentLoad = _load.Value;
        var currentView = _configureDataGridCollectionView.Value;
        var sortDescriptions = currentView.SortDescriptions.ToArray();
        var currentItems = currentLoad?.VmDataGridItemsSource;

        if (currentItems is null)
        {
            _load.Value = refreshedLoad;
            _configureDataGridCollectionView.ResetCache();
            DataGridCollectionViewMachines = _configureDataGridCollectionView.Value;
            DataGridCollectionViewMachines.SortDescriptions.Clear();
            foreach (var sortDescription in sortDescriptions)
            {
                DataGridCollectionViewMachines.SortDescriptions.Add(sortDescription);
            }
        }
        else
        {
            ReconcileMachines(currentItems, refreshedLoad.VmDataGridItemsSource ?? []);
            UpdateLoadSummary(refreshedLoad, currentItems);
            refreshedLoad.VmDataGridItemsSource = currentItems;
            _load.Value = refreshedLoad;
            currentView.Refresh();
        }

        _loadSearchOsItems.ResetCache();
        this.RaisePropertyChanged(nameof(SearchOsIsEnabled));
        this.RaisePropertyChanged(nameof(SearchOsItemCollection));
        SearchOsText = searchOsText;
        SearchFilterText = searchFilterText;
        _filterDataGridCollectionView.RunFor((searchOsText, searchFilterText));

        if (selectedMachinePath != null)
        {
            SelectedMachine = DataGridCollectionViewMachines
                             .Cast<Machine>()
                             .FirstOrDefault(machine => _pathComparer.Equals(machine.Path, selectedMachinePath));
            if (SelectedMachine != null)
            {
                DataGridCollectionViewMachines.MoveCurrentTo(SelectedMachine);
            }
        }

        this.RaisePropertyChanged(nameof(SearchFilterIsReadOnly));
        this.RaisePropertyChanged(nameof(UpdateAllIsEnabled));
        this.RaisePropertyChanged(nameof(UpdateAllTextBlockText));
        this.RaisePropertyChanged(nameof(UpdateAllHwVersionValue));
    }

    private void ReconcileMachines(ObservableCollection<Machine> currentItems, ObservableCollection<Machine> refreshedItems)
    {
        var refreshedByPath = refreshedItems
                             .Where(machine => !string.IsNullOrWhiteSpace(machine.Path))
                             .GroupBy(machine => machine.Path, _pathComparer)
                             .ToDictionary(group => group.Key, group => group.Last(), _pathComparer);
        var existingPaths = new HashSet<string>(_pathComparer);

        for (var index = currentItems.Count - 1; index >= 0; index--)
        {
            var currentMachine = currentItems[index];
            var machinePath = currentMachine.Path;
            if (string.IsNullOrWhiteSpace(machinePath))
            {
                continue;
            }

            if (!existingPaths.Add(machinePath))
            {
                currentItems.RemoveAt(index);
                continue;
            }

            if (refreshedByPath.Remove(machinePath, out var refreshedMachine))
            {
                currentMachine.ApplyFrom(refreshedMachine);
                continue;
            }

            if (IsMachineFileDeleted(currentMachine))
            {
                currentItems.RemoveAt(index);
            }
        }

        foreach (var machine in refreshedByPath.Values)
        {
            currentItems.Add(machine);
        }
    }

    private static void UpdateLoadSummary(LoadHelper load, IEnumerable<Machine> machines)
    {
        var machinesSnapshot = machines.ToArray();
        var searchOsItems = new ConcurrentDictionary<string, bool>();

        foreach (var machine in machinesSnapshot)
        {
            if (!string.IsNullOrWhiteSpace(machine.GuestOs))
            {
                searchOsItems.TryAdd(machine.GuestOs, true);
            }
        }

        load.SearchOsItems = searchOsItems;
        load.UpdateAllTextBlocks = machinesSnapshot.Length == 0
                                       ? string.Empty
                                       : $"Update all {machinesSnapshot.Length} machines to version";
        load.UpdateAllHwVersion = machinesSnapshot.Length == 0
                                      ? null
                                      : machinesSnapshot.Max(machine => machine.HwVersion);
    }

    private static bool IsMachineFileDeleted(Machine machine)
    {
        if (!Directory.Exists(machine.Directory))
        {
            return false;
        }

        try
        {
            File.GetAttributes(machine.Path);
            return false;
        }
        catch (FileNotFoundException)
        {
            return true;
        }
        catch (DirectoryNotFoundException)
        {
            return true;
        }
        catch (Exception exception) when (exception is IOException or UnauthorizedAccessException or System.Security.SecurityException)
        {
            return false;
        }
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