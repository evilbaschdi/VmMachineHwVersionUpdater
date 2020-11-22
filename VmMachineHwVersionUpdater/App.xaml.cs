using System.Windows;

#if (!DEBUG)
using ControlzEx.Theming;

#endif

namespace VmMachineHwVersionUpdater
{
    /// <inheritdoc />
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class App : Application
    {
#if (!DEBUG)
        /// <inheritdoc />
        protected override void OnStartup(StartupEventArgs e)
        {
            ThemeManager.Current.SyncTheme(ThemeSyncMode.SyncAll);

            base.OnStartup(e);
        }
#endif
    }
}