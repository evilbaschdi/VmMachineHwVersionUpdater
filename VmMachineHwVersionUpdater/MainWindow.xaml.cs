using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shell;
using EvilBaschdi.Core.Application;
using EvilBaschdi.Core.Browsers;
using EvilBaschdi.Core.Wpf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using VmMachineHwVersionUpdater.Core;
using VmMachineHwVersionUpdater.Internal;
using VmMachineHwVersionUpdater.Model;

namespace VmMachineHwVersionUpdater
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class MainWindow : MetroWindow
    {
        private Machine _currentMachine;
        private readonly IMetroStyle _style;
        private IHardwareVersion _hardwareVersion;
        private IEnumerable<Machine> _currentItemSource;
        private IEnumerable<Machine> _filteredItemSource;
        private readonly IGuestOsOutputStringMapping _guestOsOutputStringMapping;
        private readonly IAppSettings _settings;
        private readonly IDialogService _dialogService;
        private readonly int _overrideProtection;
        private int _updateAllHwVersion;
        private string _dragAndDropPath;

        #region General

        /// <summary>
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            ISettings coreSettings = new CoreSettings(Properties.Settings.Default);
            IThemeManagerHelper themeManagerHelper = new ThemeManagerHelper();
            _settings = new AppSettings();
            _style = new MetroStyle(this, Accent, ThemeSwitch, coreSettings, themeManagerHelper);
            _style.Load(true, true);
            _dialogService = new DialogService(this);
            _guestOsOutputStringMapping = new GuestOsOutputStringMapping();

            if (!string.IsNullOrWhiteSpace(_settings.VMwarePool) && Directory.Exists(_settings.VMwarePool))
            {
                VmPath.Text = _settings.VMwarePool;
                LoadAsync();
            }
            else
            {
                ToggleSettingsFlyout();
            }
            var linkerTime = Assembly.GetExecutingAssembly().GetLinkerTime();
            LinkerTime.Content = linkerTime.ToString(CultureInfo.InvariantCulture);
            _overrideProtection = 1;
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
            await task;

            var loadHelper = task.Result;
            DataContext = _currentItemSource;
            VmDataGrid.ItemsSource = loadHelper.VmDataGridItemsSource;
            UpdateAllTextBlock.Text = loadHelper.UpdateAllTextBlox;
            UpdateAllHwVersion.Value = loadHelper.UpdateAllHwVersion;
            loadHelper.SearchOsItems.ForEach(x => SearchOs.Items.Add(x));

            SearchOs.Text = "(no filter)";
            SearchFilter.Text = string.Empty;

            SearchFilter.IsReadOnly = !_currentItemSource.Any();
            SearchOs.IsEnabled = _currentItemSource.Any();
            UpdateAll.IsEnabled = _currentItemSource.Any();
        }

        private LoadHelper LoadGrid()
        {
            var loadHelper = new LoadHelper();
            _hardwareVersion = new HardwareVersion(_guestOsOutputStringMapping);
            _currentItemSource = _hardwareVersion.ReadFromPath(VMwarePoolPath()).ToList();

            if (_currentItemSource.Any())
            {
                var searchOsItems = new List<string>();
                foreach (var machine in _currentItemSource.OrderBy(m => m.GuestOs).Where(item => !searchOsItems.Contains(item.GuestOs)))
                {
                    searchOsItems.Add(machine.GuestOs);
                }

                loadHelper.UpdateAllTextBlox = $"Update all {_currentItemSource.Count()} machines to version";
                loadHelper.VmDataGridItemsSource = _currentItemSource.OrderBy(vm => vm.DisplayName).ToList();
                loadHelper.UpdateAllHwVersion = _currentItemSource.Select(machine => machine.HwVersion).Max();
                loadHelper.SearchOsItems = searchOsItems;
            }
            return loadHelper;
        }

        private string VMwarePoolPath()
        {
            return !string.IsNullOrWhiteSpace(_dragAndDropPath) ? _dragAndDropPath : _settings.VMwarePool;
        }

        private void SearchOnTextChanged(object sender, TextChangedEventArgs e)
        {
            FilterItemSource();
        }

        private void SearchOsOnDropDownClosed(object sender, EventArgs e)
        {
            FilterItemSource();
        }

        private void FilterItemSource()
        {
            _filteredItemSource = _currentItemSource;

            if (!string.IsNullOrWhiteSpace(SearchFilter.Text))
            {
                _filteredItemSource = _filteredItemSource.Where(vm => vm.DisplayName.ToLower().Contains(SearchFilter.Text.ToLower()));
            }

            if (SearchOs.Text != "(no filter)")
            {
                _filteredItemSource = _filteredItemSource.Where(vm => vm.GuestOs.ToLower().Equals(SearchOs.Text.ToLower()));
            }

            DataContext = _filteredItemSource;
            VmDataGrid.ItemsSource = _filteredItemSource.OrderBy(vm => vm.DisplayName).ToList();
        }


        private void VmDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VmDataGrid.SelectedItem == null)
            {
                return;
            }
            _currentMachine = (Machine) VmDataGrid.SelectedItem;
        }

        private void BrowseClick(object sender, RoutedEventArgs e)
        {
            var currentVmwarePool = _settings.VMwarePool;
            VmPath.Text = _settings.VMwarePool;

            var browser = new ExplorerFolderBrowser
                          {
                              SelectedPath = VmPath.Text
                          };
            browser.ShowDialog();
            _settings.VMwarePool = browser.SelectedPath;
            VmPath.Text = browser.SelectedPath;
            if (!string.Equals(currentVmwarePool, _settings.VMwarePool, StringComparison.CurrentCultureIgnoreCase) && Directory.Exists(_settings.VMwarePool))
            {
                LoadGrid();
            }
        }

        #endregion General

        #region Update

        private async void UpdateAllClick(object sender, RoutedEventArgs e)
        {
            await ConfigureUpdate();
        }

        private async Task ConfigureUpdate()
        {
            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Indeterminate;
            Cursor = Cursors.Wait;

            var task = Task.Factory.StartNew(Update);
            await task;

            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
            Cursor = Cursors.Arrow;
            LoadGrid();
        }

        private void Update()
        {
            var version = _updateAllHwVersion;
            var localList = _currentItemSource.Where(vm => vm.HwVersion != version);
            Parallel.ForEach(localList, machine => { _hardwareVersion.Update(machine.Path, version); });
        }

        private void UpdateAllHwVersionOnValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (UpdateAllHwVersion.Value != null)
            {
                _updateAllHwVersion = (int) UpdateAllHwVersion.Value;
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

        private async void DeleteClick(object sender, RoutedEventArgs e)
        {
            await DeleteClickAsync();
        }

        private async Task DeleteClickAsync()
        {
            var result = await _dialogService.ShowMessage("Delete machine...", $"Are you sure you want to delete '{_currentMachine.DisplayName}'",
                MessageDialogStyle.AffirmativeAndNegative);
            if (result == MessageDialogResult.Affirmative)
            {
                CallDelete();
            }
        }

        private void CallDelete()
        {
            if (!File.Exists(_currentMachine.Path))
            {
                return;
            }
            var path = Path.GetDirectoryName(_currentMachine.Path);
            if (!string.IsNullOrWhiteSpace(path))
            {
                try
                {
                    Directory.Delete(path);
                    LoadAsync();
                }
                catch (IOException ioException)
                {
                    _dialogService.ShowMessage("'Delete machine' was canceled", ioException.Message);
                }
            }
        }

        # endregion VM Tools

        #region Settings

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            ToggleSettingsFlyout();
        }

        private void ToggleSettingsFlyout()
        {
            var flyout = (Flyout) Flyouts.Items[0];

            if (flyout == null)
            {
                return;
            }

            flyout.IsOpen = !flyout.IsOpen;
        }

        #endregion Settings

        #region MetroStyle

        private void SaveStyleClick(object sender, RoutedEventArgs e)
        {
            if (_overrideProtection == 0)
            {
                return;
            }
            _style.SaveStyle();
        }

        private void Theme(object sender, EventArgs e)
        {
            if (_overrideProtection == 0)
            {
                return;
            }
            var routedEventArgs = e as RoutedEventArgs;
            if (routedEventArgs != null)
            {
                _style.SetTheme(sender, routedEventArgs);
            }
            else
            {
                _style.SetTheme(sender);
            }
        }

        private void AccentOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_overrideProtection == 0)
            {
                return;
            }
            _style.SetAccent(sender, e);
        }

        #endregion MetroStyle

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
                                MessageBox.Show(ex.InnerException.Message + " - " + ex.InnerException.StackTrace);
                            }
                            MessageBox.Show(ex.Message + " - " + ex.StackTrace);
                            throw;
                        }
                    }
                }
            }
            e.Handled = true;
        }

        private void GridOnDragOver(object sender, DragEventArgs e)
        {
            bool isCorrect = true;

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