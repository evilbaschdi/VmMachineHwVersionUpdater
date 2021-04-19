using System;
using EvilBaschdi.CoreExtended.AppHelpers;
using JetBrains.Annotations;
using MahApps.Metro.Controls.Dialogs;
using VmMachineHwVersionUpdater.Core.PerMachine;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc />
    public class InitDefaultCommands : IInitDefaultCommands
    {
        private readonly IArchiveMachine _archiveMachine;
        private readonly ICurrentItemSource _currentItemSource;
        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly IInit _init;
        private readonly IProcessByPath _processByPath;
        private readonly ISelectedMachine _selectedMachine;
        private readonly ITaskbarItemProgressState _taskbarItemProgressState;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="dialogCoordinator"></param>
        /// <param name="selectedMachine"></param>
        /// <param name="archiveMachine"></param>
        /// <param name="init"></param>
        /// <param name="processByPath"></param>
        /// <param name="currentItemSource"></param>
        /// <param name="taskbarItemProgressState"></param>
        public InitDefaultCommands([NotNull] IDialogCoordinator dialogCoordinator,
                                   [NotNull] ISelectedMachine selectedMachine,
                                   [NotNull] IArchiveMachine archiveMachine,
                                   [NotNull] IInit init,
                                   [NotNull] IProcessByPath processByPath,
                                   [NotNull] ICurrentItemSource currentItemSource,
                                   [NotNull] ITaskbarItemProgressState taskbarItemProgressState)
        {
            _dialogCoordinator = dialogCoordinator ?? throw new ArgumentNullException(nameof(dialogCoordinator));
            _selectedMachine = selectedMachine ?? throw new ArgumentNullException(nameof(selectedMachine));
            _archiveMachine = archiveMachine ?? throw new ArgumentNullException(nameof(archiveMachine));
            _init = init ?? throw new ArgumentNullException(nameof(init));
            _processByPath = processByPath ?? throw new ArgumentNullException(nameof(processByPath));
            _currentItemSource = currentItemSource ?? throw new ArgumentNullException(nameof(currentItemSource));
            _taskbarItemProgressState = taskbarItemProgressState ?? throw new ArgumentNullException(nameof(taskbarItemProgressState));
        }

        /// <inheritdoc />
        public IAboutWindowClickDefaultCommand AboutWindowClickDefaultCommand { get; set; }

        /// <inheritdoc />
        public IArchiveDefaultCommand ArchiveDefaultCommand { get; set; }

        /// <inheritdoc />
        public IOpenWithCodeDefaultCommand OpenWithCodeDefaultCommand { get; set; }

        /// <inheritdoc />
        public IAddEditAnnotationDefaultCommand AddEditAnnotationDefaultCommand { get; set; }

        /// <inheritdoc />
        public IDeleteDefaultCommand DeleteDefaultCommand { get; set; }

        /// <inheritdoc />
        public IGotToDefaultCommand GotToDefaultCommand { get; set; }

        /// <inheritdoc />
        public IReloadDefaultCommand ReloadDefaultCommand { get; set; }

        /// <inheritdoc />
        public IStartDefaultCommand StartDefaultCommand { get; set; }

        /// <inheritdoc />
        public IUpdateAllDefaultCommand UpdateAllDefaultCommand { get; set; }

        /// <inheritdoc />
        public object DialogCoordinatorContext { get; set; }

        /// <inheritdoc />
        public void Run()
        {
            AboutWindowClickDefaultCommand = new AboutWindowClickDefaultCommand();
            ArchiveDefaultCommand = new ArchiveDefaultCommand(_dialogCoordinator, _selectedMachine, _archiveMachine, _init);
            OpenWithCodeDefaultCommand = new OpenWithCodeDefaultCommand(_selectedMachine, _processByPath);
            AddEditAnnotationDefaultCommand = new AddEditAnnotationDefaultCommand(_selectedMachine, _init);
            ReloadDefaultCommand = new ReloadDefaultCommand(_dialogCoordinator, _processByPath);
            DeleteDefaultCommand = new DeleteDefaultCommand(_dialogCoordinator, _selectedMachine, _init, ReloadDefaultCommand);
            GotToDefaultCommand = new GotToDefaultCommand(_selectedMachine, _processByPath);

            StartDefaultCommand = new StartDefaultCommand(_processByPath, _selectedMachine);
            UpdateAllDefaultCommand = new UpdateAllDefaultCommand(_init, _currentItemSource, _taskbarItemProgressState);

            ArchiveDefaultCommand.DialogCoordinatorContext = DialogCoordinatorContext;
            DeleteDefaultCommand.DialogCoordinatorContext = DialogCoordinatorContext;
            ReloadDefaultCommand.DialogCoordinatorContext = DialogCoordinatorContext;
        }
    }
}