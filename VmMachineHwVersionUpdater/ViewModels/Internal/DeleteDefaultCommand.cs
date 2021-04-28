using System;
using System.IO;
using System.Threading.Tasks;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using JetBrains.Annotations;
using MahApps.Metro.Controls.Dialogs;
using VmMachineHwVersionUpdater.Core.PerMachine;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc />
    public class DeleteDefaultCommand : IDeleteDefaultCommand
    {
        private readonly IDeleteMachine _deleteMachine;
        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly IReloadDefaultCommand _reloadDefaultCommand;
        private readonly ISelectedMachine _selectedMachine;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="dialogCoordinator"></param>
        /// <param name="selectedMachine"></param>
        /// <param name="deleteMachine"></param>
        /// <param name="reloadDefaultCommand"></param>
        public DeleteDefaultCommand([NotNull] IDialogCoordinator dialogCoordinator, [NotNull] ISelectedMachine selectedMachine, [NotNull] IDeleteMachine deleteMachine,
                                    [NotNull] IReloadDefaultCommand reloadDefaultCommand)
        {
            _dialogCoordinator = dialogCoordinator ?? throw new ArgumentNullException(nameof(dialogCoordinator));
            _selectedMachine = selectedMachine ?? throw new ArgumentNullException(nameof(selectedMachine));
            _deleteMachine = deleteMachine ?? throw new ArgumentNullException(nameof(deleteMachine));
            _reloadDefaultCommand = reloadDefaultCommand ?? throw new ArgumentNullException(nameof(reloadDefaultCommand));
        }

        /// <inheritdoc />
        public DefaultCommand Value => new()
                                       {
                                           Command = new RelayCommand(async _ => await RunTask())
                                       };


        /// <inheritdoc />
        public async Task RunTask()
        {
            var result = await _dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "Delete machine...",
                $"Are you sure you want to delete '{_selectedMachine.Value.DisplayName}'?",
                MessageDialogStyle.AffirmativeAndNegative).ConfigureAwait(true);

            if (result == MessageDialogResult.Affirmative)
            {
                try
                {
                    _deleteMachine.RunFor(_selectedMachine.Value.Path);

                    await _reloadDefaultCommand.RunTask();
                }
                catch (IOException ioException)
                {
                    await _dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "'Delete machine' was canceled", ioException.Message);
                }
            }
        }

        /// <inheritdoc />
        public object DialogCoordinatorContext { get; set; }
    }
}