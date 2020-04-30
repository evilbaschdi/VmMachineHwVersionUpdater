using System.Windows;
using ControlzEx.Theming;

namespace VmMachineHwVersionUpdater
{
    /// <inheritdoc />
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class App : Application
    {
        /// <inheritdoc />
        protected override void OnStartup(StartupEventArgs e)
        {
            
            ThemeManager.Current.SyncTheme(ThemeSyncMode.SyncAll);

            base.OnStartup(e);
        }
    }
}