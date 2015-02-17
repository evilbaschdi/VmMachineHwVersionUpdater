using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using MahApps.Metro.Controls;
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

        public MainWindow()
        {
            InitializeComponent();

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
            VmDataGrid.ItemsSource = hardwareVersion.ReadFromPath(Properties.Settings.Default.VMwarePool);
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
        }

        private void VmDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(VmDataGrid.SelectedItem == null)
            {
                return;
            }
            _currentMachine = (Machine) VmDataGrid.SelectedItem;
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {
            StartVm();
        }

        private void VmDataGridOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            StartVm();
        }

        private void GoToClick(object sender, RoutedEventArgs e)
        {
            if(File.Exists(_currentMachine.Path))
            {
                var path = Path.GetDirectoryName(_currentMachine.Path);
                if(!string.IsNullOrWhiteSpace(path) && Directory.Exists(path))
                {
                    Process.Start(path);
                }
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
    }
}