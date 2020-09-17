using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using VmMachineHwVersionUpdater.ViewModels;

namespace VmMachineHwVersionUpdater
{
    /// <inheritdoc cref="MetroWindow" />
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class MainWindow : MetroWindow
    {
        private readonly IMainWindowViewModel _mainWindowViewModel;

        /// <inheritdoc />
        /// <summary>
        ///     Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            _mainWindowViewModel = new MainWindowViewModel(DialogCoordinator.Instance);
            Loaded += MainWindowLoaded;
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = _mainWindowViewModel;
            _mainWindowViewModel.Run();
        }
    }
}