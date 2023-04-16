using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls.ApplicationLifetimes;
using EvilBaschdi.About.Avalonia;
using EvilBaschdi.Core.AppHelpers;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using VmMachineHwVersionUpdater.Core.BasicApplication;
using VmMachineHwVersionUpdater.Core.Models;

// ReSharper disable UnusedMember.Global

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <inheritdoc />
public class MainWindowViewModel : ViewModelBase
{
    private readonly IConfigureDataGridCollectionView _configureDataGridCollectionView;
    private readonly ICurrentItem _currentItem;
    private readonly IFilterDataGridCollectionView _filterDataGridCollectionView;
    private readonly ILoad _load;
    private readonly ILoadSearchOsItems _loadSearchOsItems;
    private readonly IProcessByPath _processByPath;
    private readonly IServiceProvider _serviceProvider;
    private string _searchFilterText = string.Empty;
    private string _searchOsText = string.Empty;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="load"></param>
    /// <param name="currentItem"></param>
    /// <param name="processByPath"></param>
    /// <param name="loadSearchOsItems"></param>
    /// <param name="configureDataGridCollectionView"></param>
    /// <param name="filterDataGridCollectionView"></param>
    /// <param name="serviceProvider"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public MainWindowViewModel([NotNull] ILoad load,
                               [NotNull] ICurrentItem currentItem,
                               [NotNull] IProcessByPath processByPath,
                               [NotNull] ILoadSearchOsItems loadSearchOsItems,
                               [NotNull] IConfigureDataGridCollectionView configureDataGridCollectionView,
                               [NotNull] IFilterDataGridCollectionView filterDataGridCollectionView,
                               [NotNull] IServiceProvider serviceProvider)

    {
        _load = load ?? throw new ArgumentNullException(nameof(load));
        _currentItem = currentItem ?? throw new ArgumentNullException(nameof(currentItem));
        _processByPath = processByPath ?? throw new ArgumentNullException(nameof(processByPath));
        _loadSearchOsItems = loadSearchOsItems ?? throw new ArgumentNullException(nameof(loadSearchOsItems));
        _configureDataGridCollectionView = configureDataGridCollectionView ?? throw new ArgumentNullException(nameof(configureDataGridCollectionView));
        _filterDataGridCollectionView = filterDataGridCollectionView ?? throw new ArgumentNullException(nameof(filterDataGridCollectionView));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

        StartCommand = ReactiveCommand.Create<Machine>(StartCommandDummy);
        AboutWindowCommand = ReactiveCommand.Create(AboutWindowCommandDummy);
        UpdateAllCommand = ReactiveCommand.Create(UpdateAllCommandDummy);
    }

    /// <summary>
    ///     Binding
    /// </summary>
    public DataGridCollectionView DataGridCollectionViewMachines
    {
        get => _configureDataGridCollectionView.Value;
        set => _configureDataGridCollectionView.Value = value;
    }

    private void UpdateAllCommandDummy()
    {
        throw new NotImplementedException();
    }

    private void AboutWindowCommandDummy()
    {
        var aboutWindow = _serviceProvider.GetRequiredService<AboutWindow>();
        var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;
        if (mainWindow != null)
        {
            aboutWindow.ShowDialog(mainWindow);
        }
    }

    private void StartCommandDummy(Machine currentItem)
    {
        _currentItem.Value = currentItem;
        _processByPath.RunFor(_currentItem.Value.Path);
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
    public ReactiveCommand<Machine, Unit> StartCommand { get; }

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