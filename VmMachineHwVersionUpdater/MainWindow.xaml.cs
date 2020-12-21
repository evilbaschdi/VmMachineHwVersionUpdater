using System;
using System.Windows;
using EvilBaschdi.CoreExtended.AppHelpers;
using JetBrains.Annotations;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using VmMachineHwVersionUpdater.Core.PerMachine;
using VmMachineHwVersionUpdater.Core.Settings;
using VmMachineHwVersionUpdater.ViewModels;
using VmMachineHwVersionUpdater.ViewModels.Internal;

namespace VmMachineHwVersionUpdater
{
    /// <inheritdoc cref="MetroWindow" />
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class MainWindow : MetroWindow
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        /// <inheritdoc />
        /// <summary>
        ///     Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            // ReSharper disable once SuggestVarOrType_SimpleTypes
            IDialogCoordinator dialogCoordinator = DialogCoordinator.Instance;
            ISelectedMachine selectedMachine = new SelectedMachine();
            IVmPools vmPools = new VmPools();
            IPathSettings pathSettings = new PathSettings(vmPools);
            IArchiveMachine archiveMachine = new ArchiveMachine(pathSettings);
            IProcessByPath processByPath = new ProcessByPath();
            IInit init = new Init(dialogCoordinator, pathSettings);
            ICurrentItemSource currentItemSource = new CurrentItemSource(init);
            IConfigureListCollectionView configureListCollectionView = new ConfigureListCollectionView(init);
            IFilterItemSource filterItemSource = new FilterItemSource(configureListCollectionView);
            ITaskbarItemProgressState taskbarItemProgressState = new CurrentTaskbarItemProgressState();
            IInitDefaultCommands initDefaultCommands =
                new InitDefaultCommands(dialogCoordinator, selectedMachine, archiveMachine, init, processByPath, currentItemSource, taskbarItemProgressState);
            ILoadSearchOsItems loadSearchOsItems = new LoadSearchOsItems(init);
            _mainWindowViewModel = new MainWindowViewModel(
                selectedMachine,
                initDefaultCommands,
                init,
                configureListCollectionView,
                filterItemSource,
                currentItemSource,
                loadSearchOsItems,
                taskbarItemProgressState);
            Loaded += MainWindowLoaded;
        }

        private void MainWindowLoaded([NotNull] object sender, [NotNull] RoutedEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            _mainWindowViewModel.Run();
            DataContext = _mainWindowViewModel;
        }
    }
}