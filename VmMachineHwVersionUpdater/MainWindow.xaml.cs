using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MahApps.Metro.Controls;
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
            }
            else
            {
                //ShowErrorDialog();
                var settings = new Settings();
                settings.Show();
                settings.BringIntoView();
            }
        }

        private void LoadGrid()
        {
            var hardwareVersion = new HardwareVersion();
            VmDataGrid.ItemsSource = hardwareVersion.ReadFromPath(Properties.Settings.Default.VMwarePool);
        }

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            var settings = new Settings();
            settings.Show();
            settings.Closed += (o, args) => LoadGrid();
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

        //private async void ShowErrorDialog()
        //{
        //    var options = new MetroDialogSettings
        //    {
        //        ColorScheme = MetroDialogColorScheme.Theme
        //    };

        //    MetroDialogOptions = options;

        //    await
        //        this.ShowMessageAsync("Please set your vmware parent folder.",
        //            "This is the folder where all your vmware machines are located.");
        //}

        //public async void ShowUpdateDialog(string displayName, int newVersion)
        //{
        //    var options = new MetroDialogSettings
        //    {
        //        ColorScheme = MetroDialogColorScheme.Theme
        //    };

        //    MetroDialogOptions = options;

        //    await this.ShowMessageAsync(string.Format("The hardware version of '{0}' was changed:", displayName),
        //        string.Format("New Hardwareversion: '{0}'", newVersion));
        //}
    }
}