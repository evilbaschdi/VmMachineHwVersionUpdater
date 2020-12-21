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
    public class ArchiveDefaultCommand : IArchiveDefaultCommand
    {
        private readonly IArchiveMachine _archiveMachine;
        private readonly IInit _init;
        [NotNull] private readonly IDialogCoordinator _instance;
        private readonly ISelectedMachine _selectedMachine;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="selectedMachine"></param>
        /// <param name="archiveMachine"></param>
        /// <param name="instance"></param>
        /// <param name="init"></param>
        public ArchiveDefaultCommand([NotNull] IDialogCoordinator instance, [NotNull] ISelectedMachine selectedMachine, [NotNull] IArchiveMachine archiveMachine,
                                     [NotNull] IInit init)
        {
            _selectedMachine = selectedMachine ?? throw new ArgumentNullException(nameof(selectedMachine));
            _archiveMachine = archiveMachine ?? throw new ArgumentNullException(nameof(archiveMachine));
            _init = init ?? throw new ArgumentNullException(nameof(init));
            _instance = instance ?? throw new ArgumentNullException(nameof(instance));
        }

        /// <inheritdoc />
        public async Task RunTask()
        {
            var result = await _instance.ShowMessageAsync(DialogCoordinatorContext, "Archive machine...",
                $"Are you sure you want to archive machine '{_selectedMachine.Value.DisplayName}'?",
                MessageDialogStyle.AffirmativeAndNegative).ConfigureAwait(true);

            if (result == MessageDialogResult.Affirmative)
            {
                try
                {
                    _archiveMachine.RunFor(_selectedMachine.Value);
                }
                catch (IOException ioException)
                {
                    await _instance.ShowMessageAsync(DialogCoordinatorContext, "'Archive machine' was canceled", ioException.Message);
                }
                catch (Exception exception)
                {
                    await _instance.ShowMessageAsync(DialogCoordinatorContext, "'Archive machine' was canceled", exception.Message);
                }

                _init.Run();
            }
        }

        /// <inheritdoc />
        public DefaultCommand Value => new()
                                       {
                                           Command = new RelayCommand(async _ => await RunTask())
                                       };

        /// <inheritdoc />
        public object DialogCoordinatorContext { get; set; }
    }
}