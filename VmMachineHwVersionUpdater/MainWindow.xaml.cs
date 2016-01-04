using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EvilBaschdi.Core.Application;
using EvilBaschdi.Core.Wpf;
using MahApps.Metro.Controls;
using VmMachineHwVersionUpdater.Core;
using VmMachineHwVersionUpdater.Internal;

namespace VmMachineHwVersionUpdater
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable RedundantExtendsListEntry
    public partial class MainWindow : MetroWindow
        // ReSharper restore RedundantExtendsListEntry
    {
        private Machine _currentMachine;
        private readonly IMetroStyle _style;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ISettings _coreSettings;
        private readonly IAppBasics _basics;
        private IHardwareVersion _hardwareVersion;
        private IEnumerable<Machine> _currentItemSource;
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly IAppSettings _settings;
        private int _overrideProtection;

        #region General

        /// <summary>
        /// </summary>
        public MainWindow()
        {
            _settings = new AppSettings();
            _coreSettings = new CoreSettings();
            _basics = new AppBasics(_settings);
            InitializeComponent();
            _style = new MetroStyle(this, Accent, Dark, Light, _coreSettings);
            _style.Load(true, false);

            if(!string.IsNullOrWhiteSpace(Properties.Settings.Default.VMwarePool))
            {
                //LoadGrid();
                VmPath.Text = Properties.Settings.Default.VMwarePool;
            }
            else
            {
                ToggleSettingsFlyout();
            }
        }

        private void LoadClick(object sender, RoutedEventArgs e)
        {
            LoadGrid();
        }

        private void LoadGrid()
        {
            _hardwareVersion = new HardwareVersion();
            _currentItemSource = _hardwareVersion.ReadFromPath(_basics.GetVMwarePool());
            var currentItemSource = _currentItemSource as IList<Machine> ?? _currentItemSource.ToList();
            DataContext = currentItemSource;
            VmDataGrid.ItemsSource = currentItemSource.OrderBy(vm => vm.DisplayName).ToList();

            if(currentItemSource.Any())
            {
                UpdateAllTextBlock.Text = $"Update all {currentItemSource.Count} machines to version";
                GetLatestHwVersionForUpdateAll();
            }

            _overrideProtection = 1;
        }

        private void VmDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(VmDataGrid.SelectedItem == null)
            {
                return;
            }
            _currentMachine = (Machine) VmDataGrid.SelectedItem;
        }

        private void BrowseClick(object sender, RoutedEventArgs e)
        {
            _basics.BrowseFolder();
            VmPath.Text = _basics.GetVMwarePool();
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
            var version = Convert.ToInt32(UpdateAllHwVersion.Value);
            var localList = _currentItemSource.Where(vm => vm.HwVersion != version);
            Parallel.ForEach(localList, machine => { _hardwareVersion.Update(machine.Path, version); });
            //todo: handling taskleiste usw.
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
            if(!File.Exists(_currentMachine.Path))
            {
                return;
            }
            var path = Path.GetDirectoryName(_currentMachine.Path);
            if(!string.IsNullOrWhiteSpace(path) && Directory.Exists(path))
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

            if(flyout == null)
            {
                return;
            }

            flyout.IsOpen = !flyout.IsOpen;
            flyout.ClosingFinished += SaveSettings;
        }

        private void SaveSettings(object sender, RoutedEventArgs e)
        {
            LoadGrid();
        }

        #endregion Settings

        #region MetroStyle

        private void SaveStyleClick(object sender, RoutedEventArgs e)
        {
            if(_overrideProtection == 0)
            {
                return;
            }
            _style.SaveStyle();
        }

        private void Theme(object sender, RoutedEventArgs e)
        {
            if(_overrideProtection == 0)
            {
                return;
            }
            _style.SetTheme(sender, e);
        }

        private void AccentOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(_overrideProtection == 0)
            {
                return;
            }
            _style.SetAccent(sender, e);
        }

        #endregion MetroStyle
    }
}