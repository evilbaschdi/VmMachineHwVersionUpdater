using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls.ApplicationLifetimes;
using EvilBaschdi.About.Avalonia;
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

        AboutWindowCommand = ReactiveCommand.Create(AboutWindowCommandAction);
        UpdateAllCommand = ReactiveCommand.Create(UpdateAllCommandAction);

        Run();
    }

    /// <inheritdoc />
    public void Run()
    {
        _initReactiveCommands.Run();

        OpenWithCodeCommand = _initReactiveCommands.OpenWithCodeReactiveCommand.ReactiveCommandValue;
        StartCommand = _initReactiveCommands.StartReactiveCommand.ReactiveCommandValue;
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

    private void UpdateAllCommandAction()
    {
        throw new NotImplementedException();
    }

    private void AboutWindowCommandAction()
    {
        var aboutWindow = App.ServiceProvider.GetRequiredService<AboutWindow>();
        var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;
        if (mainWindow != null)
        {
            aboutWindow.ShowDialog(mainWindow);
        }
    }

    #region Commands

    /// <summary>
    /// </summary>
    public ReactiveCommand<Unit, Unit> AboutWindowCommand { get; set; }

    /// <summary>
    /// </summary>
    public ReactiveCommand<Unit, Unit> UpdateAllCommand { get; set; }

    /// <summary>
    /// </summary>
    public ReactiveCommand<Unit, Unit> StartCommand { get; set; }

    /// <summary>
    /// </summary>
    public ReactiveCommand<Unit, Unit> OpenWithCodeCommand { get; set; }

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