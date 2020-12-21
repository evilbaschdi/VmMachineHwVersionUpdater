using System;
using System.IO;
using System.Threading.Tasks;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using JetBrains.Annotations;
using MahApps.Metro.Controls.Dialogs;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc />
    public class DeleteDefaultCommand : IDeleteDefaultCommand
    {
        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly IInit _init;
        private readonly ISelectedMachine _selectedMachine;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="dialogCoordinator"></param>
        /// <param name="selectedMachine"></param>
        /// <param name="init"></param>
        public DeleteDefaultCommand([NotNull] IDialogCoordinator dialogCoordinator, [NotNull] ISelectedMachine selectedMachine, [NotNull] IInit init)
        {
            _dialogCoordinator = dialogCoordinator ?? throw new ArgumentNullException(nameof(dialogCoordinator));
            _selectedMachine = selectedMachine ?? throw new ArgumentNullException(nameof(selectedMachine));
            _init = init ?? throw new ArgumentNullException(nameof(init));
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
                    _init.DeleteMachine.RunFor(_selectedMachine.Value.Path);
                }
                catch (IOException ioException)
                {
                    await _dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "'Delete machine' was canceled", ioException.Message);
                }

                _init.Run();
            }
        }

        /// <inheritdoc />
        public object DialogCoordinatorContext { get; set; }
    }
}