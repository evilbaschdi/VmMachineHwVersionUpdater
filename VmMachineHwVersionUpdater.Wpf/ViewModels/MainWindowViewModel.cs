﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Shell;
using EvilBaschdi.Core.Wpf;
using EvilBaschdi.Core.Wpf.Mvvm.ViewModel;
using VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

namespace VmMachineHwVersionUpdater.Wpf.ViewModels;

/// <inheritdoc cref="IMainWindowViewModel" />
/// <inheritdoc cref="INotifyPropertyChanged" />
/// <summary>
///     MainWindowViewModel of VmMachineHwVersionUpdater.
/// </summary>
public class MainWindowViewModel : ApplicationLayoutViewModel, IMainWindowViewModel
{
    private readonly IConfigureListCollectionView _configureListCollectionView;
    private readonly ICurrentMachine _currentMachine;
    private readonly IFilterListCollectionView _filterListCollectionView;
    private readonly IInitDefaultCommands _initDefaultCommands;
    private readonly ILoad _load;
    private readonly ILoadSearchOsItems _loadSearchOsItems;
    private readonly ITaskbarItemProgressState _taskbarItemProgressState;

    #region Constructor

    /// <summary>
    ///     Constructor
    /// </summary>
    public MainWindowViewModel(
        IApplicationLayout applicationLayout,
        IApplicationStyle applicationStyle,
        ICurrentMachine currentMachine,
        IInitDefaultCommands initDefaultCommands,
        ILoad load,
        IConfigureListCollectionView configureListCollectionView,
        IFilterListCollectionView filterListCollectionView,
        ICurrentItemSource currentItemSource,
        ILoadSearchOsItems loadSearchOsItems,
        ITaskbarItemProgressState taskbarItemProgressState
    )
        : base(applicationLayout, applicationStyle, true, true)
    {
        _currentMachine = currentMachine ?? throw new ArgumentNullException(nameof(currentMachine));
        _initDefaultCommands = initDefaultCommands ?? throw new ArgumentNullException(nameof(initDefaultCommands));
        _load = load ?? throw new ArgumentNullException(nameof(load));
        _configureListCollectionView = configureListCollectionView ?? throw new ArgumentNullException(nameof(configureListCollectionView));
        _filterListCollectionView = filterListCollectionView ?? throw new ArgumentNullException(nameof(filterListCollectionView));
        CurrentItemSource = currentItemSource ?? throw new ArgumentNullException(nameof(currentItemSource));
        _loadSearchOsItems = loadSearchOsItems ?? throw new ArgumentNullException(nameof(loadSearchOsItems));
        _taskbarItemProgressState = taskbarItemProgressState ?? throw new ArgumentNullException(nameof(taskbarItemProgressState));

        Run();
    }

    /// <inheritdoc />
    public void Run()
    {
        _initDefaultCommands.DialogCoordinatorContext = this;
        _initDefaultCommands.ArchiveDefaultCommand.DialogCoordinatorContext = this;
        _initDefaultCommands.CopyDefaultCommand.DialogCoordinatorContext = this;
        _initDefaultCommands.DeleteDefaultCommand.DialogCoordinatorContext = this;
        _initDefaultCommands.ReloadDefaultCommand.DialogCoordinatorContext = this;
        _initDefaultCommands.RenameDefaultCommand.DialogCoordinatorContext = this;

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
        get => _currentMachine.Value;
        set
        {
            _currentMachine.Value = value;
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
        get;
        set
        {
            field = value;
            _filterListCollectionView.RunFor((SearchOsText, value));
            OnPropertyChanged();
        }
    } = string.Empty;

    /// <summary>
    ///     Binding
    /// </summary>
    public string SearchOsText
    {
        get;
        set
        {
            field = value;
            _filterListCollectionView.RunFor((value, SearchFilterText));
            OnPropertyChanged();
        }
    } = string.Empty;

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
        get => _load.Value?.VmDataGridItemsSource != null && _load.Value.VmDataGridItemsSource.Any();
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