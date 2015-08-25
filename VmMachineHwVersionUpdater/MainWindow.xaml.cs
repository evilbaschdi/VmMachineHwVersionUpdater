using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using MahApps.Metro.Controls;
using VmMachineHwVersionUpdater.Core;
using VmMachineHwVersionUpdater.Extensions;
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
        private readonly ApplicationStyle _style;
        private IEnumerable<Machine> _currentItemSource;

        public MainWindow()
        {
            _style = new ApplicationStyle(this);
            InitializeComponent();
            _style.Load();

            if(!string.IsNullOrWhiteSpace(Properties.Settings.Default.VMwarePool))
            {
                LoadGrid();
                VmPath.Text = Properties.Settings.Default.VMwarePool;
            }
            else
            {
                ToggleSettingsFlyout();
            }
        }

        private void LoadGrid()
        {
            var hardwareVersion = new HardwareVersion();
            _currentItemSource = hardwareVersion.ReadFromPath(Properties.Settings.Default.VMwarePool);
            VmDataGrid.ItemsSource = _currentItemSource;
        }

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
            Properties.Settings.Default.VMwarePool = VmPath.Text;
            Properties.Settings.Default.Save();
            LoadGrid();
        }

        private void VmDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(VmDataGrid.SelectedItem == null)
            {
                return;
            }
            _currentMachine = (Machine) VmDataGrid.SelectedItem;
        }

        private void UpdateAllClick(object sender, RoutedEventArgs e)
        {
            foreach(var machine in _currentItemSource)
            {
                machine.HwVersion = Convert.ToInt32(UpdateAllHwVersion.Value);
            }
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {
            StartVm();
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

        private void BrowseClick(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog(this.GetIWin32Window());

            VmPath.Text = folderBrowserDialog.SelectedPath;
        }

        private void SaveStyleClick(object sender, RoutedEventArgs e)
        {
            _style.SaveStyle();
        }

        private void Theme(object sender, RoutedEventArgs e)
        {
            _style.SetTheme(sender, e);
        }

        private void AccentOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _style.SetAccent(sender, e);
        }
    }
}