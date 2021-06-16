using System;
using System.IO;
using System.Threading.Tasks;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using JetBrains.Annotations;
using MahApps.Metro.Controls.Dialogs;
using VmMachineHwVersionUpdater.Core.Models;
using VmMachineHwVersionUpdater.Core.PerMachine;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc />
    public class ArchiveDefaultCommand : IArchiveDefaultCommand
    {
        private readonly IArchiveMachine _archiveMachine;
        [NotNull] private readonly IDialogCoordinator _dialogCoordinator;
        [NotNull] private readonly IReloadDefaultCommand _reloadDefaultCommand;
        private readonly ICurrentItem _currentItem;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="reloadDefaultCommand"></param>
        /// <param name="currentItem"></param>
        /// <param name="archiveMachine"></param>
        /// <param name="instance"></param>
        public ArchiveDefaultCommand([NotNull] IDialogCoordinator instance, [NotNull] IReloadDefaultCommand reloadDefaultCommand, [NotNull] ICurrentItem currentItem,
                                     [NotNull] IArchiveMachine archiveMachine)
        {
            _currentItem = currentItem ?? throw new ArgumentNullException(nameof(currentItem));
            _archiveMachine = archiveMachine ?? throw new ArgumentNullException(nameof(archiveMachine));

            _dialogCoordinator = instance ?? throw new ArgumentNullException(nameof(instance));
            _reloadDefaultCommand = reloadDefaultCommand ?? throw new ArgumentNullException(nameof(reloadDefaultCommand));
        }

        /// <inheritdoc />
        public async Task RunAsync()
        {
            var result = await _dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "Archive machine...",
                $"Are you sure you want to archive machine '{_currentItem.Value.DisplayName}'?",
                MessageDialogStyle.AffirmativeAndNegative).ConfigureAwait(true);

            if (result == MessageDialogResult.Affirmative)
            {
                try
                {
                    _archiveMachine.RunFor(_currentItem.Value);
                }
                catch (IOException ioException)
                {
                    await _dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "'Archive machine' was canceled", ioException.Message);
                }
                catch (Exception exception)
                {
                    await _dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "'Archive machine' was canceled", exception.Message);
                }

                await _reloadDefaultCommand.RunAsync();
            }
        }

        /// <inheritdoc />
        public DefaultCommand Value => new()
                                       {
                                           Command = new RelayCommand(async _ => await RunAsync())
                                       };

        /// <inheritdoc />
        public object DialogCoordinatorContext { get; set; }
    }
}