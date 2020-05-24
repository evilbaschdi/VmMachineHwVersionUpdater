using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shell;
using EvilBaschdi.Core.Extensions;
using EvilBaschdi.Core.Internal;
using EvilBaschdi.CoreExtended;
using EvilBaschdi.CoreExtended.AppHelpers;
using EvilBaschdi.CoreExtended.Controls.About;
using JetBrains.Annotations;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using VmMachineHwVersionUpdater.Core.BasicApplication;
using VmMachineHwVersionUpdater.Core.Models;
using VmMachineHwVersionUpdater.Core.PerMachine;
using VmMachineHwVersionUpdater.Core.Settings;

namespace VmMachineHwVersionUpdater
{
    /// <inheritdoc cref="MetroWindow" />
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class MainWindow : MetroWindow
    {
        private readonly IPathSettings _pathSettings;
        private IArchiveMachine _archiveMachine;
        private List<Machine> _currentItemSource;
        private IDeleteMachine _deleteMachine;
        private IGuestOsesInUse _guestOsesInUse;
        private ListCollectionView _listCollectionView;
        private IMachinesFromPath _machinesFromPath;
        private string _prevSortHeader;
        private IProcessByPath _processByPath;
        private SortDescription _sd = new SortDescription("DisplayName", ListSortDirection.Ascending);
        private Machine _selectedMachine;
        private string _sortHeader;
        private IToggleToolsSyncTime _toggleToolsSyncTime;
        private IToggleToolsUpgradePolicy _toggleToolsUpgradePolicy;
        private int _updateAllHwVersion;
        private IUpdateMachineVersion _updateMachineVersion;

        #region General

        /// <inheritdoc />
        /// <summary>
        ///     Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            IVmPools vmPools = new VmPools();
            _pathSettings = new PathSettings(vmPools);
            var applicationStyle = new ApplicationStyle();
            applicationStyle.Load(true, true);

            var vmPoolFromSettingExistingPaths = _pathSettings.VmPool.GetExistingDirectories();
            if (vmPoolFromSettingExistingPaths.Any())
            {
                Load();
            }
        }

        private void LoadClick(object sender, RoutedEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            Load();
        }

        private void Load()
        {
            IFileListFromPath fileListFromPath = new FileListFromPath();
            IGuestOsStringMapping guestOsStringMapping = new GuestOsStringMapping();
            IGuestOsOutputStringMapping guestOsOutputStringMapping = new GuestOsOutputStringMapping(guestOsStringMapping);
            IReadLogInformation readLogInformation = new ReadLogInformation();
            _updateMachineVersion = new UpdateMachineVersion();

            IHandleMachineFromPath handleMachineFromPath = new HandleMachineFromPath(guestOsOutputStringMapping, _pathSettings, _updateMachineVersion, readLogInformation);
            _processByPath = new ProcessByPath();
            _toggleToolsSyncTime = new ToggleToolsSyncTime();
            _toggleToolsUpgradePolicy = new ToggleToolsUpgradePolicy();
            _guestOsesInUse = new GuestOsesInUse(guestOsStringMapping);
            _machinesFromPath = new MachinesFromPath(_pathSettings, handleMachineFromPath, fileListFromPath);
            _archiveMachine = new ArchiveMachine(_pathSettings);
            _deleteMachine = new DeleteMachine();

            ILoad load = new Load(_machinesFromPath);
            var loadValue = load.Value;

            _currentItemSource = loadValue.VmDataGridItemsSource;
            DataContext = loadValue.VmDataGridItemsSource;
            _listCollectionView = new ListCollectionView(loadValue.VmDataGridItemsSource);
            _listCollectionView.GroupDescriptions.Add(new PropertyGroupDescription("Directory"));
            _listCollectionView.SortDescriptions.Add(_sd);
            VmDataGrid.ItemsSource = _listCollectionView;

            UpdateAllTextBlock.Text = loadValue.UpdateAllTextBlocks;
            UpdateAllHwVersion.Value = loadValue.UpdateAllHwVersion;

            //SearchOs filter
            LoadSearchOsItems(loadValue);
        }

        private void LoadSearchOsItems([NotNull] LoadHelper loadValue)
        {
            if (loadValue == null)
            {
                throw new ArgumentNullException(nameof(loadValue));
            }

            SearchOs.Items.Clear();
            SearchOs.Items.Add("(no filter)");
            SearchOs.Items.Add(new Separator());
            loadValue.SearchOsItems.ForEach(x => SearchOs.Items.Add(x));
            SearchOs.Items.Add(new Separator());
            _guestOsesInUse.Value.ForEach(x => SearchOs.Items.Add(x));

            SearchOs.Text = "(no filter)";
            SearchFilter.Text = string.Empty;

            SearchFilter.IsReadOnly = !loadValue.VmDataGridItemsSource.Any();
            SearchOs.IsEnabled = loadValue.VmDataGridItemsSource.Any();
            UpdateAll.IsEnabled = loadValue.VmDataGridItemsSource.Any();
        }

        private void VmDataGridSorting(object sender, DataGridSortingEventArgs e)
        {
            _sortHeader = e.Column.SortMemberPath;

            _sd = _sortHeader == _prevSortHeader
                ? new SortDescription(_sortHeader, ListSortDirection.Descending)
                : new SortDescription(_sortHeader, ListSortDirection.Ascending);
            _prevSortHeader = _sortHeader;
        }


        private void SearchOsOnDropDownClosed(object sender, EventArgs e)
        {
            FilterItemSource();
        }

        private void SearchOnTextChanged(object sender, TextChangedEventArgs e)
        {
            FilterItemSource();
        }

        private void FilterItemSource()
        {
            if (SearchOs.Text != "(no filter)")
            {
                _listCollectionView.Filter = vm => ((Machine) vm).GuestOs.StartsWith(SearchOs.Text, StringComparison.InvariantCultureIgnoreCase);
            }
            else
            {
                _listCollectionView.Filter = vm => true;
            }

            if (!string.IsNullOrWhiteSpace(SearchFilter.Text))
            {
                _listCollectionView.Filter = vm => ((Machine) vm).DisplayName.Contains(SearchFilter.Text, StringComparison.InvariantCultureIgnoreCase);
            }

            VmDataGrid.ItemsSource = _listCollectionView;
        }


        private void VmDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VmDataGrid.SelectedItem == null)
            {
                return;
            }

            _selectedMachine = (Machine) VmDataGrid.SelectedItem;
        }

        private void AboutWindowClick(object sender, RoutedEventArgs e)
        {
            var assembly = typeof(MainWindow).Assembly;
            IAboutContent aboutWindowContent = new AboutContent(assembly, $@"{AppDomain.CurrentDomain.BaseDirectory}\b.png");

            var aboutWindow = new AboutWindow
                              {
                                  DataContext = new AboutViewModel(aboutWindowContent)
                              };

            aboutWindow.ShowDialog();
        }

        #endregion General

        #region Update

        private async void UpdateAllClick(object sender, RoutedEventArgs e)
        {
            await ConfigureUpdateAsync().ConfigureAwait(true);
        }

        private async Task ConfigureUpdateAsync()
        {
            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Indeterminate;
            Cursor = Cursors.Wait;

            var task = Task.Factory.StartNew(Update);
            await task.ConfigureAwait(true);

            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
            Cursor = Cursors.Arrow;
            Load();
        }

        private void Update()
        {
            var version = _updateAllHwVersion;
            var localList = _currentItemSource.AsParallel().Where(vm => vm.HwVersion != version).ToList();

            _updateMachineVersion.RunFor(localList, version);
        }

        private void UpdateAllHwVersionOnValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (UpdateAllHwVersion.Value != null)
            {
                _updateAllHwVersion = (int) UpdateAllHwVersion.Value;
            }
        }

        private void SyncTimeWithHostCheckBoxClick(object sender, RoutedEventArgs e)
        {
            var checkBox = ((CheckBox) sender).IsChecked;
            if (checkBox.HasValue)
            {
                _toggleToolsSyncTime.RunFor(_selectedMachine.Path, checkBox.Value);
            }
        }

        private void AutoUpdateToolsCheckBoxClick(object sender, RoutedEventArgs e)
        {
            var checkBox = ((CheckBox) sender).IsChecked;
            if (checkBox.HasValue)
            {
                _toggleToolsUpgradePolicy.RunFor(_selectedMachine.Path, checkBox.Value);
            }
        }

        #endregion Update

        #region VM Tools

        private void StartClick(object sender, RoutedEventArgs e)
        {
            StartVm();
        }

        private void OpenWithCodeClick(object sender, RoutedEventArgs e)
        {
            OpenWithCode();
        }

        private void StartVm()
        {
            _processByPath.RunFor(_selectedMachine.Path);
        }

        private void OpenWithCode()
        {
            _processByPath.RunFor($"vscode://file/{_selectedMachine.Path}");
        }

        private void GoToClick(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(_selectedMachine.Path))
            {
                return;
            }

            var path = Path.GetDirectoryName(_selectedMachine.Path);
            if (!string.IsNullOrWhiteSpace(path) && Directory.Exists(path))
            {
                _processByPath.RunFor(path);
            }
        }

        private async void ArchiveClick(object sender, RoutedEventArgs e)
        {
            await ArchiveClickAsync().ConfigureAwait(true);
        }

        private async void DeleteClick(object sender, RoutedEventArgs e)
        {
            await DeleteClickAsync().ConfigureAwait(true);
        }

        private async Task ArchiveClickAsync()
        {
            var result = await this.ShowMessageAsync("Archive machine...",
                $"Are you sure you want to archive machine '{_selectedMachine.DisplayName}'?",
                MessageDialogStyle.AffirmativeAndNegative).ConfigureAwait(true);

            if (result == MessageDialogResult.Affirmative)
            {
                try
                {
                    _archiveMachine.RunFor(_selectedMachine);
                }
                catch (IOException ioException)
                {
                    await this.ShowMessageAsync("'Archive machine' was canceled", ioException.Message);
                }
                catch (Exception exception)
                {
                    await this.ShowMessageAsync("'Archive machine' was canceled", exception.Message);
                }

                Load();
            }
        }

        private async Task DeleteClickAsync()
        {
            var result = await this.ShowMessageAsync("Delete machine...",
                $"Are you sure you want to delete '{_selectedMachine.DisplayName}'?",
                MessageDialogStyle.AffirmativeAndNegative).ConfigureAwait(true);
            if (result == MessageDialogResult.Affirmative)
            {
                try
                {
                    _deleteMachine.RunFor(_selectedMachine.Path);
                }
                catch (IOException ioException)
                {
                    await this.ShowMessageAsync("'Delete machine' was canceled", ioException.Message);
                }

                Load();
            }
        }

        # endregion VM Tools
    }
}