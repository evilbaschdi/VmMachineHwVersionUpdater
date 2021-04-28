using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Shell;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using VmMachineHwVersionUpdater.Core.BasicApplication;
using VmMachineHwVersionUpdater.Core.Models;
using VmMachineHwVersionUpdater.ViewModels.Internal;

namespace VmMachineHwVersionUpdater.ViewModels
{
    /// <inheritdoc cref="INotifyPropertyChanged" />
    /// <summary>
    ///     MainWindowViewModel of VmMachineHwVersionUpdater.
    /// </summary>
    public class MainWindowViewModel : ApplicationStyleViewModel, IMainWindowViewModel
    {
        private readonly IConfigureListCollectionView _configureListCollectionView;


        private readonly IFilterItemSource _filterItemSource;

        //private readonly IInit _init;
        private readonly IInitDefaultCommands _initDefaultCommands;
        private readonly ILoad _load;
        private readonly ILoadSearchOsItems _loadSearchOsItems;
        private readonly ISelectedMachine _selectedMachine;
        private readonly ITaskbarItemProgressState _taskbarItemProgressState;
        private string _searchFilterText = string.Empty;
        private string _searchOsText = "(no filter)";

        #region Constructor

        /// <summary>
        ///     Constructor
        /// </summary>
        public MainWindowViewModel(
            ISelectedMachine selectedMachine,
            IInitDefaultCommands initDefaultCommands,
            //IInit init,
            ILoad load,
            IConfigureListCollectionView configureListCollectionView,
            IFilterItemSource filterItemSource,
            ICurrentItemSource currentItemSource,
            ILoadSearchOsItems loadSearchOsItems,
            ITaskbarItemProgressState taskbarItemProgressState
        )
            : base(true, true)
        {
            _selectedMachine = selectedMachine ?? throw new ArgumentNullException(nameof(selectedMachine));
            _initDefaultCommands = initDefaultCommands ?? throw new ArgumentNullException(nameof(initDefaultCommands));
            //_init = init ?? throw new ArgumentNullException(nameof(init));
            _load = load ?? throw new ArgumentNullException(nameof(load));
            _configureListCollectionView = configureListCollectionView ?? throw new ArgumentNullException(nameof(configureListCollectionView));
            _filterItemSource = filterItemSource ?? throw new ArgumentNullException(nameof(filterItemSource));
            CurrentItemSource = currentItemSource ?? throw new ArgumentNullException(nameof(currentItemSource));
            _loadSearchOsItems = loadSearchOsItems ?? throw new ArgumentNullException(nameof(loadSearchOsItems));
            _taskbarItemProgressState = taskbarItemProgressState ?? throw new ArgumentNullException(nameof(taskbarItemProgressState));

            Run();
        }

        #endregion Constructor

        /// <inheritdoc />
        public void Run()
        {
            //_init.DialogCoordinatorContext = this;
            //_init.RunFor(this);
            _initDefaultCommands.DialogCoordinatorContext = this;
            _initDefaultCommands.Run();

            AboutWindowClick = _initDefaultCommands.AboutWindowClickDefaultCommand.Value;
            AddEditAnnotation = _initDefaultCommands.AddEditAnnotationDefaultCommand.Value;
            Archive = _initDefaultCommands.ArchiveDefaultCommand.Value;
            Delete = _initDefaultCommands.DeleteDefaultCommand.Value;
            GoTo = _initDefaultCommands.GotToDefaultCommand.Value;
            OpenWithCode = _initDefaultCommands.OpenWithCodeDefaultCommand.Value;
            Reload = _initDefaultCommands.ReloadDefaultCommand.Value;
            Start = _initDefaultCommands.StartDefaultCommand.Value;
            UpdateAll = _initDefaultCommands.UpdateAllDefaultCommand.Value;
        }


        #region Commands

        /// <summary />
        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        public ICommandViewModel AboutWindowClick { get; set; }

        /// <summary />
        public ICommandViewModel AddEditAnnotation { get; set; }

        /// <summary />
        public ICommandViewModel Archive { get; set; }

        /// <summary />
        public ICommandViewModel Delete { get; set; }

        /// <summary />
        public ICommandViewModel GoTo { get; set; }

        /// <summary />
        public ICommandViewModel OpenWithCode { get; set; }

        /// <summary />
        public ICurrentItemSource CurrentItemSource { get; }

        /// <summary />
        public ICommandViewModel Reload { get; set; }

        /// <summary />
        public ICommandViewModel Start { get; set; }

        /// <summary />
        public ICommandViewModel UpdateAll { get; set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global
        // ReSharper restore MemberCanBePrivate.Global

        #endregion Commands

        #region Properties

        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable UnusedMember.Global
        /// <summary>
        ///     Binding
        /// </summary>
        public Machine SelectedMachine
        {
            get => _selectedMachine.Value;
            set
            {
                _selectedMachine.Value = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Binding
        /// </summary>
        public ListCollectionView ListCollectionView

        {
            get
            {
                _configureListCollectionView.DialogCoordinatorContext = this;
                return _configureListCollectionView.Value;
            }
            set
            {
                _configureListCollectionView.Value = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        ///     Binding
        /// </summary>
        public string SearchFilterText
        {
            get => _searchFilterText;
            set
            {
                _searchFilterText = value;
                _filterItemSource.RunFor(SearchOsText, value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Binding
        /// </summary>
        public string SearchOsText
        {
            get => _searchOsText;
            set
            {
                _searchOsText = value;
                _filterItemSource.RunFor(value, SearchFilterText);
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Binding
        /// </summary>
        public ObservableCollection<object> SearchOsItemCollection
        {
            get => _loadSearchOsItems.Value;
            set
            {
                _loadSearchOsItems.Value = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        ///     Binding
        /// </summary>
        public double? UpdateAllHwVersionValue
        {
            get => _load.Value.UpdateAllHwVersion;
            set
            {
                _load.Value.UpdateAllHwVersion = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Binding for UpdateAllTextBlock
        /// </summary>
        public string UpdateAllTextBlockText
        {
            get => _load.Value.UpdateAllTextBlocks;
            set
            {
                _load.Value.UpdateAllTextBlocks = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Binding
        /// </summary>
        public bool SearchOsIsEnabled
        {
            get => _load.Value.VmDataGridItemsSource.Any();
            // ReSharper disable once ValueParameterNotUsed
            set => OnPropertyChanged();
        }

        /// <summary>
        ///     Binding
        /// </summary>
        public bool SearchFilterIsReadOnly
        {
            get => !_load.Value.VmDataGridItemsSource.Any();
            // ReSharper disable once ValueParameterNotUsed
            set => OnPropertyChanged();
        }

        /// <summary>
        ///     Binding
        /// </summary>
        public bool UpdateAllIsEnabled
        {
            get => _load.Value.VmDataGridItemsSource.Any();
            // ReSharper disable once ValueParameterNotUsed
            set => OnPropertyChanged();
        }

        /// <summary>
        ///     Binding
        /// </summary>
        public TaskbarItemProgressState ProgressState
        {
            get => _taskbarItemProgressState.Value;

            set
            {
                _taskbarItemProgressState.Value = value;
                OnPropertyChanged();
            }
        }
        // ReSharper restore UnusedMember.Global
        // ReSharper restore MemberCanBePrivate.Global

        #endregion Properties
    }
}