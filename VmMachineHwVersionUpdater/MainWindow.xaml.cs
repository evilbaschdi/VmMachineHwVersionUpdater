using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using VmMachineHwVersionUpdater.Core;
using VmMachineHwVersionUpdater.Internal;

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
        private readonly BackgroundWorker _bw;
        private readonly int _overrideProtection;
        private int _executionCount;
        private int _updateAllHwVersion;

        #region General

        /// <summary>
        /// </summary>
        public MainWindow()
        {
            _settings = new AppSettings();
            var coreSettings = new CoreSettings();
            InitializeComponent();
            _bw = new BackgroundWorker();
            _style = new MetroStyle(this, Accent, ThemeSwitch, coreSettings);
            _style.Load(true, true);
            _guestOsOutputStringMapping = new GuestOsOutputStringMapping();

            if (!string.IsNullOrWhiteSpace(_settings.VMwarePool) && Directory.Exists(_settings.VMwarePool))
            {
                VmPath.Text = _settings.VMwarePool;
                LoadGrid();
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

            LoadGrid();
        }

        private void LoadGrid()
        {
            _hardwareVersion = new HardwareVersion(_guestOsOutputStringMapping);
            _currentItemSource = _hardwareVersion.ReadFromPath(_settings.VMwarePool).ToList();
            DataContext = _currentItemSource;
            VmDataGrid.ItemsSource = _currentItemSource.OrderBy(vm => vm.DisplayName).ToList();

            if (_currentItemSource.Any())
            {
                foreach (var machine in _currentItemSource.OrderBy(m => m.GuestOs))
                {
                    if (!SearchOs.Items.Contains(machine.GuestOs))
                    {
                        SearchOs.Items.Add(machine.GuestOs);
                    }
                }

                UpdateAllTextBlock.Text = $"Update all {_currentItemSource.Count()} machines to version";
                GetLatestHwVersionForUpdateAll();
                SearchFilter.IsReadOnly = false;
                SearchOs.Text = "(no filter)";
                SearchFilter.Text = string.Empty;
                SearchOs.IsEnabled = true;
                UpdateAll.IsEnabled = true;
            }
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

        private void GetLatestHwVersionForUpdateAll()
        {
            var latest = _currentItemSource.Select(machine => machine.HwVersion).Max();
            UpdateAllHwVersion.Value = latest;
        }

        private void UpdateAllClick(object sender, RoutedEventArgs e)
        {
            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Indeterminate;
            ConfigureUpdate();
        }

        private void ConfigureUpdate()
        {
            _executionCount++;
            Cursor = Cursors.Wait;
            _updateAllHwVersion = Convert.ToInt32(UpdateAllHwVersion.Value);
            if (_executionCount == 1)
            {
                _bw.DoWork += (o, args) => Update();
                _bw.WorkerReportsProgress = true;
                _bw.RunWorkerCompleted += BackgroundWorkerRunWorkerCompleted;
            }
            _bw.RunWorkerAsync();
        }

        private void Update()
        {
            var version = _updateAllHwVersion;
            var localList = _currentItemSource.Where(vm => vm.HwVersion != version);
            Parallel.ForEach(localList, machine => { _hardwareVersion.Update(machine.Path, version); });
        }

        private void BackgroundWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
            Cursor = Cursors.Arrow;
            LoadGrid();
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
    }
}