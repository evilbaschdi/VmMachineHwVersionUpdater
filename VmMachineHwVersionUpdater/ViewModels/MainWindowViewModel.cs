using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shell;
using EvilBaschdi.Core.Extensions;
using EvilBaschdi.Core.Internal;
using EvilBaschdi.CoreExtended.AppHelpers;
using EvilBaschdi.CoreExtended.Controls.About;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using JetBrains.Annotations;
using MahApps.Metro.Controls.Dialogs;
using VmMachineHwVersionUpdater.Core.BasicApplication;
using VmMachineHwVersionUpdater.Core.Models;
using VmMachineHwVersionUpdater.Core.PerMachine;
using VmMachineHwVersionUpdater.Core.Settings;

namespace VmMachineHwVersionUpdater.ViewModels
{
    /// <inheritdoc cref="INotifyPropertyChanged" />
    /// <summary>
    ///     MainWindowViewModel of VmMachineHwVersionUpdater.
    /// </summary>
    public class MainWindowViewModel : ApplicationStyleViewModel, IMainWindowViewModel

    {
        private readonly IDialogCoordinator _instance;
        private readonly SortDescription _sd = new("DisplayName", ListSortDirection.Ascending);
        private IArchiveMachine _archiveMachine;
        private List<Machine> _currentItemSource;
        private IDeleteMachine _deleteMachine;
        private IGuestOsesInUse _guestOsesInUse;
        private ListCollectionView _listCollectionView;
        private IMachinesFromPath _machinesFromPath;
        private IPathSettings _pathSettings;
        private IProcessByPath _processByPath;
        private TaskbarItemProgressState _progressState;
        private bool _searchFilterIsReadOnly;
        private string _searchFilterText;
        private bool _searchOsIsEnabled;
        private ObservableCollection<object> _searchOsItemCollection = new();
        private string _searchOsText;
        private Machine _selectedMachine;
        private IToggleToolsSyncTime _toggleToolsSyncTime;
        private IToggleToolsUpgradePolicy _toggleToolsUpgradePolicy;
        private double? _updateAllHwVersion;
        private bool _updateAllIsEnabled;
        private string _updateAllTextBlock;
        private IUpdateMachineVersion _updateMachineVersion;

        #region Constructor

        /// <summary>
        ///     Constructor
        /// </summary>
        protected internal MainWindowViewModel([NotNull] IDialogCoordinator instance)
            : base(true, true)
        {
            _instance = instance ?? throw new ArgumentNullException(nameof(instance));
            AboutWindowClick = new DefaultCommand
                               {
                                   Text = "about",
                                   Command = new RelayCommand(_ => AboutWindowCommand())
                               };
            AddEditAnnotation = new DefaultCommand
                                {
                                    Command = new RelayCommand(_ => LoadAddEditAnnotationDialog())
                                };
            Archive = new DefaultCommand
                      {
                          Command = new RelayCommand(async _ => await ArchiveClickAsync().ConfigureAwait(true))
                      };

            Delete = new DefaultCommand
                     {
                         Command = new RelayCommand(async _ => await DeleteClickAsync().ConfigureAwait(true))
                     };

            GoTo = new DefaultCommand
                   {
                       Command = new RelayCommand(_ => GoToPath())
                   };
            OpenWithCode = new DefaultCommand
                           {
                               Command = new RelayCommand(_ => OpenVmxWithCode())
                           };
            Reload = new DefaultCommand
                     {
                         Command = new RelayCommand(_ => Load())
                     };
            Start = new DefaultCommand
                    {
                        Command = new RelayCommand(_ => StartVm())
                    };
            UpdateAll = new DefaultCommand
                        {
                            Command = new RelayCommand(async _ => await ConfigureUpdateAsync().ConfigureAwait(true))
                        };
        }

        #endregion Constructor

        #region Commands

        /// <summary />
        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        // ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
        public ICommandViewModel AboutWindowClick { get; init; }

        /// <summary />
        public ICommandViewModel AddEditAnnotation { get; init; }

        /// <summary />
        public ICommandViewModel Archive { get; init; }

        /// <summary />
        public ICommandViewModel Delete { get; init; }

        /// <summary />
        public ICommandViewModel GoTo { get; init; }

        /// <summary />
        public ICommandViewModel OpenWithCode { get; init; }

        /// <summary />
        public ICommandViewModel Reload { get; init; }

        /// <summary />
        public ICommandViewModel Start { get; init; }

        /// <summary />
        public ICommandViewModel UpdateAll { get; init; }
        // ReSharper restore AutoPropertyCanBeMadeGetOnly.Global
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
            get => _selectedMachine;
            set
            {
                _selectedMachine = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Binding
        /// </summary>
        public ListCollectionView ListCollectionView

        {
            get => _listCollectionView;
            set
            {
                _listCollectionView = value;
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
                FilterItemSource();
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Binding
        /// </summary>
        public ObservableCollection<object> SearchOsItemCollection
        {
            get => _searchOsItemCollection;
            set
            {
                _searchOsItemCollection = value;
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
                FilterItemSource();
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Binding
        /// </summary>
        public double? UpdateAllHwVersionValue
        {
            get => _updateAllHwVersion;
            set
            {
                _updateAllHwVersion = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Binding for UpdateAllTextBlock
        /// </summary>
        public string UpdateAllTextBlockText
        {
            get => _updateAllTextBlock;
            set
            {
                _updateAllTextBlock = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Binding
        /// </summary>
        public bool SearchOsIsEnabled
        {
            get => _searchOsIsEnabled;
            set
            {
                _searchOsIsEnabled = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Binding
        /// </summary>
        public bool SearchFilterIsReadOnly
        {
            get => _searchFilterIsReadOnly;
            set
            {
                _searchFilterIsReadOnly = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Binding
        /// </summary>
        public bool UpdateAllIsEnabled
        {
            get => _updateAllIsEnabled;
            set
            {
                _updateAllIsEnabled = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        ///     Binding
        /// </summary>
        public TaskbarItemProgressState ProgressState
        {
            get => _progressState;

            set
            {
                _progressState = value;
                OnPropertyChanged();
            }
        }
        // ReSharper restore UnusedMember.Global
        // ReSharper restore MemberCanBePrivate.Global

        #endregion Properties

        #region Load

        /// <inheritdoc />
        public void Run()
        {
            IVmPools vmPools = new VmPools();
            _pathSettings = new PathSettings(vmPools);

            var vmPoolFromSettingExistingPaths = _pathSettings.VmPool.GetExistingDirectories();
            if (!vmPoolFromSettingExistingPaths.Any())
            {
                _instance.ShowMessageAsync(this, "No virtual machines found",
                    "Please verify settings and discs attached");
                return;
            }

            Load();
        }

        private void Load()
        {
            IFileListFromPath fileListFromPath = new FileListFromPath();
            IGuestOsStringMapping guestOsStringMapping = new GuestOsStringMapping();
            IGuestOsOutputStringMapping guestOsOutputStringMapping =
                new GuestOsOutputStringMapping(guestOsStringMapping);
            IReadLogInformation readLogInformation = new ReadLogInformation();
            _updateMachineVersion = new UpdateMachineVersion();
            _toggleToolsUpgradePolicy = new ToggleToolsUpgradePolicy();
            _toggleToolsSyncTime = new ToggleToolsSyncTime();
            IReturnValueFromVmxLine returnValueFromVmxLine = new ReturnValueFromVmxLine();
            IVmxLineStartsWith vmxLineStartsWith = new VmxLineStartsWith();
            IConvertAnnotationLineBreaks convertAnnotationLineBreaks = new ConvertAnnotationLineBreaks();
            IHandleMachineFromPath handleMachineFromPath =
                new HandleMachineFromPath(guestOsOutputStringMapping, _pathSettings, _updateMachineVersion,
                    readLogInformation, returnValueFromVmxLine, vmxLineStartsWith,
                    convertAnnotationLineBreaks, _toggleToolsUpgradePolicy, _toggleToolsSyncTime);
            _processByPath = new ProcessByPath();

            _guestOsesInUse = new GuestOsesInUse(guestOsStringMapping);
            _machinesFromPath = new MachinesFromPath(_pathSettings, handleMachineFromPath, fileListFromPath);
            _archiveMachine = new ArchiveMachine(_pathSettings);
            _deleteMachine = new DeleteMachine();

            ILoad load = new Load(_machinesFromPath);
            var loadValue = load.Value;

            _currentItemSource = loadValue.VmDataGridItemsSource;
            ListCollectionView = new ListCollectionView(loadValue.VmDataGridItemsSource);
            ListCollectionView?.GroupDescriptions?.Add(new PropertyGroupDescription("Directory"));
            ListCollectionView.SortDescriptions.Add(_sd);

            UpdateAllTextBlockText = loadValue.UpdateAllTextBlocks;
            UpdateAllHwVersionValue = loadValue.UpdateAllHwVersion;

            //SearchOs filter
            LoadSearchOsItems(loadValue);
        }


        private void LoadSearchOsItems([NotNull] LoadHelper loadValue)
        {
            if (loadValue == null)
            {
                throw new ArgumentNullException(nameof(loadValue));
            }

            SearchOsItemCollection.Clear();
            SearchOsItemCollection.Add("(no filter)");
            SearchOsItemCollection.Add(new Separator());
            loadValue.SearchOsItems.ForEach(x => SearchOsItemCollection.Add(x));
            SearchOsItemCollection.Add(new Separator());
            _guestOsesInUse.Value.ForEach(x => SearchOsItemCollection.Add(x));

            SearchOsText = "(no filter)";
            SearchFilterText = string.Empty;

            SearchFilterIsReadOnly = !loadValue.VmDataGridItemsSource.Any();
            SearchOsIsEnabled = loadValue.VmDataGridItemsSource.Any();
            UpdateAllIsEnabled = loadValue.VmDataGridItemsSource.Any();
        }

        #endregion Load

        #region VM Tools

        private void StartVm()
        {
            _processByPath.RunFor(SelectedMachine.Path);
        }

        private void OpenVmxWithCode()
        {
            _processByPath.RunFor($"vscode://file/{SelectedMachine.Path}");
        }

        private void GoToPath()
        {
            if (!File.Exists(_selectedMachine.Path))
            {
                return;
            }

            var path = Path.GetDirectoryName(_selectedMachine.Path);
            if (!string.IsNullOrWhiteSpace(path) && Directory.Exists(path))
            {
                _processByPath.RunFor(path);
            }
        }

        private async Task ArchiveClickAsync()
        {
            var result = await _instance.ShowMessageAsync(this, "Archive machine...",
                $"Are you sure you want to archive machine '{_selectedMachine.DisplayName}'?",
                MessageDialogStyle.AffirmativeAndNegative).ConfigureAwait(true);

            if (result == MessageDialogResult.Affirmative)
            {
                try
                {
                    _archiveMachine.RunFor(_selectedMachine);
                }
                catch (IOException ioException)
                {
                    await _instance.ShowMessageAsync(this, "'Archive machine' was canceled", ioException.Message);
                }
                catch (Exception exception)
                {
                    await _instance.ShowMessageAsync(this, "'Archive machine' was canceled", exception.Message);
                }

                Load();
            }
        }

        private async Task DeleteClickAsync()
        {
            var result = await _instance.ShowMessageAsync(this, "Delete machine...",
                $"Are you sure you want to delete '{_selectedMachine.DisplayName}'?",
                MessageDialogStyle.AffirmativeAndNegative).ConfigureAwait(true);

            if (result == MessageDialogResult.Affirmative)
            {
                try
                {
                    _deleteMachine.RunFor(_selectedMachine.Path);
                }
                catch (IOException ioException)
                {
                    await _instance.ShowMessageAsync(this, "'Delete machine' was canceled", ioException.Message);
                }

                Load();
            }
        }

        private void LoadAddEditAnnotationDialog()
        {
            IAddEditAnnotation addEditAnnotation = new AddEditAnnotation();
            var addEditAnnotationDialog = new AddEditAnnotationDialog(addEditAnnotation, _selectedMachine)
                                          {
                                              DataContext = new AddEditAnnotationDialogViewModel()
                                          };
            addEditAnnotationDialog.Closing += AddEditAnnotationDialogClosing;
            addEditAnnotationDialog.ShowDialog();
        }

        private void AddEditAnnotationDialogClosing(object sender, CancelEventArgs e)
        {
            Load();
        }

        #endregion VM Tools

        #region General

        private static void AboutWindowCommand()
        {
            var assembly = typeof(MainWindow).Assembly;

            IAboutContent aboutWindowContent =
                new AboutContent(assembly, $@"{AppDomain.CurrentDomain.BaseDirectory}\b.png");
            var aboutWindow = new AboutWindow
                              {
                                  DataContext = new AboutViewModel(aboutWindowContent)
                              };
            aboutWindow.ShowDialog();
        }

        private void FilterItemSource()
        {
            if (SearchOsText != "(no filter)")
            {
                _listCollectionView.Filter = vm =>
                                                 ((Machine) vm).GuestOs.StartsWith(SearchOsText, StringComparison.InvariantCultureIgnoreCase);
            }
            else
            {
                _listCollectionView.Filter = _ => true;
            }

            if (!string.IsNullOrWhiteSpace(SearchFilterText))
            {
                _listCollectionView.Filter = vm =>
                                                 ((Machine) vm).DisplayName.Contains(SearchFilterText, StringComparison.InvariantCultureIgnoreCase);
            }

            ListCollectionView = _listCollectionView;
        }

        #endregion General

        #region Update

        private async Task ConfigureUpdateAsync()
        {
            ProgressState = TaskbarItemProgressState.Indeterminate;

            var task = Task.Factory.StartNew(Update);
            await task.ConfigureAwait(true);

            ProgressState = TaskbarItemProgressState.Normal;

            Load();
        }

        private void Update()
        {
            var version = _updateAllHwVersion;
            if (!version.HasValue)
            {
                return;
            }

            var innerVersion = Convert.ToInt32(version.Value);
            var localList = _currentItemSource.AsParallel().Where(vm => vm.HwVersion != innerVersion).ToList();
            _updateMachineVersion.RunFor(localList, innerVersion);
        }

        #endregion Update
    }
}