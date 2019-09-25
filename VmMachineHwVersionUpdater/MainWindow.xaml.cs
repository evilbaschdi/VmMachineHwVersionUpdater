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
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using VmMachineHwVersionUpdater.Core;
using VmMachineHwVersionUpdater.Core.Models;

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
        private readonly IGuestOsesInUse _guestOsesInUse;
        private readonly IGuestOsOutputStringMapping _guestOsOutputStringMapping;
        private readonly IPathSettings _pathSettings;
        private readonly IThemeManagerHelper _themeManagerHelper;

        private IEnumerable<Machine> _currentItemSource;
        private Machine _currentMachine;
        private string _dragAndDropPath;
        private IEnumerable<Machine> _filteredItemSource;
        private IHardwareVersion _hardwareVersion;
        private string _prevSortHeader;
        private SortDescription _sd = new SortDescription("DisplayName", ListSortDirection.Ascending);
        private string _sortHeader;
        private int _updateAllHwVersion;

        #region General

        /// <inheritdoc />
        /// <summary>
        ///     Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();


            _themeManagerHelper = new ThemeManagerHelper();
            _pathSettings = new PathSettings();
            var applicationStyle = new ApplicationStyle(_themeManagerHelper);
            applicationStyle.Load(true, true);
            _dialogService = new DialogService(this);
            _guestOsOutputStringMapping = new GuestOsOutputStringMapping();
            _guestOsesInUse = new GuestOsesInUse();

            var vmPoolFromSettingExistingPaths = _pathSettings.VmPool.GetExistingDirectories();

            if (vmPoolFromSettingExistingPaths.Any())
            {
                LoadAsync();
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

            _dragAndDropPath = string.Empty;
            LoadAsync();
        }

        private async void LoadAsync()
        {
            var task = Task.Factory.StartNew(LoadGrid);
            await task.ConfigureAwait(true);

            var loadHelper = task.Result;
            DataContext = _currentItemSource;
            var listCollectionView = new ListCollectionView(loadHelper.VmDataGridItemsSource);
            listCollectionView.GroupDescriptions.Add(new PropertyGroupDescription("Directory"));
            VmDataGrid.ItemsSource = listCollectionView;

            UpdateAllTextBlock.Text = loadHelper.UpdateAllTextBlocks;
            UpdateAllHwVersion.Value = loadHelper.UpdateAllHwVersion;

            //SearchOs filter
            SearchOs.Items.Clear();
            SearchOs.Items.Add("(no filter)");
            SearchOs.Items.Add(new Separator());
            loadHelper.SearchOsItems.ForEach(x => SearchOs.Items.Add(x));
            SearchOs.Items.Add(new Separator());
            _guestOsesInUse.Value.ForEach(x => SearchOs.Items.Add(x));

            SearchOs.Text = "(no filter)";
            SearchFilter.Text = string.Empty;

            SearchFilter.IsReadOnly = !_currentItemSource.Any();
            SearchOs.IsEnabled = _currentItemSource.Any();
            UpdateAll.IsEnabled = _currentItemSource.Any();

            VmDataGrid.Items.SortDescriptions.Clear();
            VmDataGrid.Items.SortDescriptions.Add(_sd);
        }


        private void VmDataGridSorting(object sender, DataGridSortingEventArgs e)
        {
            _sortHeader = e.Column.SortMemberPath;

            _sd = _sortHeader == _prevSortHeader
                ? new SortDescription(_sortHeader, ListSortDirection.Descending)
                : new SortDescription(_sortHeader, ListSortDirection.Ascending);
            _prevSortHeader = _sortHeader;
        }

        private LoadHelper LoadGrid()
        {
            var loadHelper = new LoadHelper();
            _hardwareVersion = new HardwareVersion(_guestOsOutputStringMapping);

            _currentItemSource = _hardwareVersion.ReadFromPath(VmPoolPath(), _pathSettings.ArchivePath).ToList();

            if (!_currentItemSource.Any())
            {
                return loadHelper;
            }

            var searchOsItems = new List<string>();
            foreach (var machine in _currentItemSource.OrderBy(m => m.GuestOs)
                                                      .Where(item => !searchOsItems.Contains(item.GuestOs)))
            {
                searchOsItems.Add(machine.GuestOs);
            }

            loadHelper.UpdateAllTextBlocks = $"Update all {_currentItemSource.Count()} machines to version";
            loadHelper.VmDataGridItemsSource = _currentItemSource.ToList();
            loadHelper.UpdateAllHwVersion = _currentItemSource.Select(machine => machine.HwVersion).Max();
            loadHelper.SearchOsItems = searchOsItems;

            return loadHelper;
        }

        private List<string> VmPoolPath()
        {
            var dragAndDropPath = new List<string>();
            if (string.IsNullOrWhiteSpace(_dragAndDropPath))
            {
                return _pathSettings.VmPool;
            }

            dragAndDropPath.Add(_dragAndDropPath);
            return dragAndDropPath;
        }

        private void SearchOnTextChanged(object sender, TextChangedEventArgs e)
        {
            FilterItemSource();
            //FixExpanderWith();
        }

        private void SearchOsOnDropDownClosed(object sender, EventArgs e)
        {
            FilterItemSource();
            //FixExpanderWith();
        }

        private void FilterItemSource()
        {
            _filteredItemSource = _currentItemSource;

            if (!string.IsNullOrWhiteSpace(SearchFilter.Text))
            {
                _filteredItemSource =
                    _filteredItemSource.Where(vm => vm.DisplayName.ToLower().Contains(SearchFilter.Text.ToLower()));
            }

            if (SearchOs.Text != "(no filter)")
            {
                _filteredItemSource = _filteredItemSource.Where(vm =>
                                                                    vm.GuestOs.StartsWith(SearchOs.Text, StringComparison.InvariantCultureIgnoreCase));
            }

            DataContext = _filteredItemSource;
            var listCollectionView = new ListCollectionView(_filteredItemSource.OrderBy(vm => vm.DisplayName).ToList());
            listCollectionView.GroupDescriptions.Add(new PropertyGroupDescription("Directory"));
            VmDataGrid.ItemsSource = listCollectionView;
            //FixExpanderWith();
        }


        private void VmDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VmDataGrid.SelectedItem == null)
            {
                return;
            }

            _currentMachine = (Machine) VmDataGrid.SelectedItem;
        }

        //private void BrowsePoolClick(object sender, RoutedEventArgs e)
        //{
        //    var oldPath = _pathSettings.VMwarePool;
        //    var currentVmwarePool = oldPath.SplitToEnumerable(";").ToArray();
        //    VmPath.Text = oldPath;

        //    var browser = new ExplorerFolderBrowser
        //                  {
        //                      SelectedPath = currentVmwarePool.First(),
        //                      Multiselect = true
        //                  };
        //    browser.ShowDialog();
        //    var newPath = string.Join(";", browser.SelectedPaths);
        //    _pathSettings.VMwarePool = newPath;
        //    VmPath.Text = newPath;

        //    if (!string.Equals(oldPath, newPath, StringComparison.CurrentCultureIgnoreCase) &&
        //        Directory.Exists(_pathSettings.VMwarePool))
        //    {
        //        LoadAsync();
        //    }
        //}

        //private void BrowseArchiveClick(object sender, RoutedEventArgs e)
        //{
        //    var oldPath = _pathSettings.ArchivePath;
        //    VmPath.Text = oldPath;

        //    var browser = new ExplorerFolderBrowser
        //                  {
        //                      SelectedPath = oldPath,
        //                      Multiselect = false
        //                  };
        //    browser.ShowDialog();
        //    var newPath = browser.SelectedPath;
        //    _pathSettings.ArchivePath = newPath;
        //    VmArchivePath.Text = newPath;

        //    if (!string.Equals(oldPath, newPath, StringComparison.CurrentCultureIgnoreCase) &&
        //        Directory.Exists(_pathSettings.ArchivePath))
        //    {
        //        LoadAsync();
        //    }
        //}

        //private void VmPathOnLostFocus(object sender, RoutedEventArgs e)
        //{
        //    var pathsFromSetting = VmPath.Text.SplitToEnumerable(";").ToList();
        //    var existingPaths = pathsFromSetting.GetExistingDirectories();
        //    if (!existingPaths.Any())
        //    {
        //        return;
        //    }

        //    _pathSettings.VMwarePool = string.Join(";", existingPaths);
        //    LoadAsync();
        //}


        //private void VmArchiveOnLostFocus(object sender, RoutedEventArgs e)
        //{
        //    var pathFromSetting = VmArchivePath.Text;

        //    if (!Directory.Exists(pathFromSetting))
        //    {
        //        return;
        //    }

        //    _pathSettings.ArchivePath = pathFromSetting;
        //    LoadAsync();
        //}

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
            LoadAsync();
        }

        private void Update()
        {
            var version = _updateAllHwVersion;
            var localList = _currentItemSource.Where(vm => vm.HwVersion != version).ToList();

            Parallel.ForEach(localList, machine => { _hardwareVersion.Update(machine.Path, version); });
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
                _hardwareVersion.EnableSyncTimeWithHost(_currentMachine.Path, checkBox.Value);
            }
        }

        private void AutoUpdateToolsCheckBoxClick(object sender, RoutedEventArgs e)
        {
            var checkBox = ((CheckBox) sender).IsChecked;
            if (checkBox.HasValue)
            {
                _hardwareVersion.EnableToolsAutoUpdate(_currentMachine.Path, checkBox.Value);
            }
        }

        #endregion Update

        #region VM Tools

        private void StartClick(object sender, RoutedEventArgs e)
        {
            StartVm();
        }

        private void StartVm()
        {
            var vm = new Process
                     {
                         StartInfo =
                         {
                             FileName = _currentMachine.Path
                         }
                     };
            vm.Start();
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
                Process.Start(path);
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
                LoadAsync();
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
                LoadAsync();
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
                LoadAsync();
            }
            catch (IOException ioException)
            {
                _dialogService.ShowMessage("'Delete machine' was canceled", ioException.Message);
            }
        }

        # endregion VM Tools

        #region Drag and Drop

        private void GridOnDrop(object sender, DragEventArgs e)
        {
            if (null != e.Data && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var droppedElements = (string[]) e.Data.GetData(DataFormats.FileDrop, true);
                if (droppedElements != null)
                {
                    if (droppedElements.Length > 1)
                    {
                        _dialogService.ShowMessage("Drag & Drop", "Please drag & drop only one item!");
                    }
                    else
                    {
                        var droppedElement = droppedElements.First();
                        try
                        {
                            var fileAttributes = File.GetAttributes(droppedElement);
                            var isDirectory = (fileAttributes & FileAttributes.Directory) == FileAttributes.Directory;
                            if (isDirectory)
                            {
                                _dragAndDropPath = droppedElement;
                                LoadAsync();
                                _dragAndDropPath = string.Empty;
                            }
                            else
                            {
                                _dialogService.ShowMessage("Drag & Drop", "Drag & drop of files is not supported!");
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ex.InnerException != null)
                            {
                                MessageBox.Show($"{ex.InnerException.Message} - {ex.InnerException.StackTrace}");
                            }

                            MessageBox.Show($"{ex.Message} - {ex.StackTrace}");
                            throw;
                        }
                    }
                }
            }

            e.Handled = true;
        }

        private void GridOnDragOver(object sender, DragEventArgs e)
        {
            var isCorrect = true;

            if (e.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                var droppedElements = (string[]) e.Data.GetData(DataFormats.FileDrop, true);
                if (droppedElements != null)
                {
                    if ((from droppedElement in droppedElements
                         let fileAttributes = File.GetAttributes(droppedElement)
                         let isDirectory = (fileAttributes & FileAttributes.Directory) == FileAttributes.Directory
                         where isDirectory
                         select droppedElement).Any(droppedElement => !Directory.Exists(droppedElement)))
                    {
                        isCorrect = false;
                    }
                }
            }

            e.Effects = isCorrect ? DragDropEffects.All : DragDropEffects.None;
            e.Handled = true;
        }

        #endregion
    }
}