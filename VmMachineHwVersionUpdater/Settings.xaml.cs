using System.Windows;
using System.Windows.Forms;
using MahApps.Metro.Controls;
using VmMachineHwVersionUpdater.Extensions;

namespace VmMachineHwVersionUpdater
{
    /// <summary>
    ///     Interaction logic for Settings.xaml
    /// </summary>
// ReSharper disable RedundantExtendsListEntry
    public partial class Settings : MetroWindow
// ReSharper restore RedundantExtendsListEntry
    {
        public Settings()
        {
            InitializeComponent();
            Path.Text = Properties.Settings.Default.VMwarePool;
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.VMwarePool = Path.Text;
            Properties.Settings.Default.Save();
        }

        private void BrowseClick(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog(this.GetIWin32Window());

            Path.Text = folderBrowserDialog.SelectedPath;
        }
    }
}