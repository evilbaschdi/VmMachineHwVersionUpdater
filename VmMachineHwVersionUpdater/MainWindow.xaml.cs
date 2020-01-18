using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shell;
using EvilBaschdi.Core.Extensions;
using EvilBaschdi.CoreExtended;
using EvilBaschdi.CoreExtended.Metro;
using EvilBaschdi.CoreExtended.Mvvm;
using EvilBaschdi.CoreExtended.Mvvm.View;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel;
using JetBrains.Annotations;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using VmMachineHwVersionUpdater.Core;
using VmMachineHwVersionUpdater.Core.Models;
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
        private readonly IDialogService _dialogService;
        private readonly IPathSettings _pathSettings;
        private readonly IThemeManagerHelper _themeManagerHelper;
        private List<Machine> _currentItemSource;
        private Machine _currentMachine;
        private IEnableSyncTimeWithHost _enableSyncTimeWithHost;
        private IEnableToolsAutoUpdate _enableToolsAutoUpdate;
        private IGuestOsesInUse _guestOsesInUse;
        private ListCollectionView _listCollectionView;
        private IMachinesFromPath _machinesFromPath;
        private string _prevSortHeader;
        private SortDescription _sd = new SortDescription("DisplayName", ListSortDirection.Ascending);
        private string _sortHeader;
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

            _themeManagerHelper = new ThemeManagerHelper();
            IVmPools vmPools = new VmPools();
            _pathSettings = new PathSettings(vmPools);
            var applicationStyle = new ApplicationStyle(_themeManagerHelper);
            applicationStyle.Load(true, true);
            _dialogService = new DialogService(this);

            var vmPoolFromSettingExistingPaths = _pathSettings.VmPool.GetExistingDirectories();

            if (vmPoolFromSettingExistingPaths.Any())
            {
                Load();
            }
        }

        /// <inheritdoc />
        protected override void OnClosed(EventArgs e)
        {
            foreach (Window currentWindow in Application.Current.Windows)
            {
                if (currentWindow != Application.Current.MainWindow)
                {
                    currentWindow.Close();
                }
            }

            base.OnClosed(e);
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
            IGuestOsStringMapping guestOsStringMapping = new GuestOsStringMapping();
            IGuestOsOutputStringMapping guestOsOutputStringMapping = new GuestOsOutputStringMapping(guestOsStringMapping);
            _updateMachineVersion = new UpdateMachineVersion();
            _enableSyncTimeWithHost = new EnableSyncTimeWithHost();
            _enableToolsAutoUpdate = new EnableToolsAutoUpdate();
            _guestOsesInUse = new GuestOsesInUse(guestOsStringMapping);
            _machinesFromPath = new MachinesFromPath(guestOsOutputStringMapping, _pathSettings, _updateMachineVersion);

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
                _listCollectionView.Filter = vm => ((Machine) vm).DisplayName.ToLower().Contains(SearchFilter.Text.ToLower());
            }

            VmDataGrid.ItemsSource = _listCollectionView;
        }


        private void VmDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VmDataGrid.SelectedItem == null)
            {
                return;
            }

            _currentMachine = (Machine) VmDataGrid.SelectedItem;
        }

        private void AboutWindowClick(object sender, RoutedEventArgs e)
        {
            var assembly = typeof(MainWindow).Assembly;
            IAboutWindowContent aboutWindowContent = new AboutWindowContent(assembly, $@"{AppDomain.CurrentDomain.BaseDirectory}\b.png");

            var aboutWindow = new AboutWindow
                              {
                                  DataContext = new AboutViewModel(aboutWindowContent, _themeManagerHelper)
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
                _enableSyncTimeWithHost.RunFor(_currentMachine.Path, checkBox.Value);
            }
        }

        private void AutoUpdateToolsCheckBoxClick(object sender, RoutedEventArgs e)
        {
            var checkBox = ((CheckBox) sender).IsChecked;
            if (checkBox.HasValue)
            {
                _enableToolsAutoUpdate.RunFor(_currentMachine.Path, checkBox.Value);
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
            var vm = new Process
                     {
                         StartInfo =
                         {
                             FileName = _currentMachine.Path,
                             UseShellExecute = true
                         }
                     };

            vm.Start();
        }

        private void OpenWithCode()
        {
            var process = new Process
                          {
                              StartInfo =
                              {
                                  FileName = $"vscode://file/{_currentMachine.Path}",
                                  UseShellExecute = true
                              }
                          };

            process.Start();
        }

        private void GoToClick(object sender, RoutedEventArgs e)
        {
            if (!File.Exists(_currentMachine.Path))
            {
                return;
            }

            var path = Path.GetDirectoryName(_currentMachine.Path);
            if (!string.IsNullOrWhiteSpace(path) && Directory.Exists(path))
            {
                var process = new Process
                              {
                                  StartInfo =
                                  {
                                      FileName = path,
                                      UseShellExecute = true
                                  }
                              };

                process.Start();
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
            var result = await _dialogService.ShowMessage("Archive machine...",
                $"Are you sure you want to archive machine '{_currentMachine.DisplayName}'?",
                MessageDialogStyle.AffirmativeAndNegative).ConfigureAwait(true);
            if (result == MessageDialogResult.Affirmative)
            {
                CallArchive();
                Load();
            }
        }

        private async Task DeleteClickAsync()
        {
            var result = await _dialogService.ShowMessage("Delete machine...",
                $"Are you sure you want to delete '{_currentMachine.DisplayName}'?",
                MessageDialogStyle.AffirmativeAndNegative).ConfigureAwait(true);
            if (result == MessageDialogResult.Affirmative)
            {
                CallDelete();
                Load();
            }
        }

        private void CallArchive()
        {
            if (!File.Exists(_currentMachine.Path))
            {
                return;
            }

            var path = Path.GetDirectoryName(_currentMachine.Path);
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            try
            {
                var machineDirectoryWithoutPath = path.ToLower().Replace($@"{_currentMachine.Directory.ToLower()}\", "");


                var archivePath = _pathSettings.ArchivePath?.FirstOrDefault(p => p.ToLower().StartsWith(_currentMachine.Directory.ToLower()));
                archivePath = string.IsNullOrWhiteSpace(archivePath) ? Path.Combine(_currentMachine.Directory.ToLower(), "_archive") : archivePath;

                var destination = Path.Combine(archivePath, machineDirectoryWithoutPath.ToLower());
                Directory.Move(path.ToLower(), destination.ToLower());
            }
            catch (IOException ioException)
            {
                _dialogService.ShowMessage("'Archive machine' was canceled", ioException.Message);
            }
            catch (Exception exception)
            {
                _dialogService.ShowMessage("'Archive machine' was canceled", exception.Message);
            }
        }

        private void CallDelete()
        {
            if (!File.Exists(_currentMachine.Path))
            {
                return;
            }

            var path = Path.GetDirectoryName(_currentMachine.Path);
            if (string.IsNullOrWhiteSpace(path))
            {
                return;
            }

            try
            {
                Directory.Delete(path, true);
                Load();
            }
            catch (IOException ioException)
            {
                _dialogService.ShowMessage("'Delete machine' was canceled", ioException.Message);
            }
        }

        # endregion VM Tools
    }
}