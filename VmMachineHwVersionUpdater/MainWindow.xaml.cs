using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
        public MainWindow()
        {
            InitializeComponent();
            var machinePath = Properties.Settings.Default.VMwarePool;

            if(!string.IsNullOrWhiteSpace(machinePath))
            {
                var hardwareVersion = new HardwareVersion();
                VmDataGrid.ItemsSource = hardwareVersion.ReadFromPath(machinePath);
            }
            else
            {
                //ShowErrorDialog();
                var settings = new Settings();
                settings.Show();
                settings.BringIntoView();
            }
        }

        private async void ShowErrorDialog()
        {
            var options = new MetroDialogSettings
            {
                ColorScheme = MetroDialogColorScheme.Theme
            };

            MetroDialogOptions = options;

            await
                this.ShowMessageAsync("Please set your vmware parent folder.",
                    "This is the folder where all your vmware machines are located.");
        }

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

        private void SettingsClick(object sender, RoutedEventArgs e)
        {
            var settings = new Settings();
            settings.Show();
        }
    }
}