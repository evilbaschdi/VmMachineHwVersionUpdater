﻿using System;
using System.IO;
using System.Threading.Tasks;
using EvilBaschdi.Core.Internal;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using JetBrains.Annotations;
using MahApps.Metro.Controls.Dialogs;
using VmMachineHwVersionUpdater.Core.Models;
using VmMachineHwVersionUpdater.Core.PerMachine;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc />
    public class CopyDefaultCommand : ICopyDefaultCommand
    {
        private readonly ICopyMachine _copyMachine;
        private readonly ICopyProgress _copyProgress;
        private readonly ICurrentItem _currentItem;
        private readonly IDialogCoordinator _dialogCoordinator;
        private readonly IReloadDefaultCommand _reloadDefaultCommand;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="reloadDefaultCommand"></param>
        /// <param name="currentItem"></param>
        /// <param name="instance"></param>
        /// <param name="copyMachine"></param>
        /// <param name="copyProgress"></param>
        public CopyDefaultCommand([NotNull] IDialogCoordinator instance, [NotNull] IReloadDefaultCommand reloadDefaultCommand, [NotNull] ICurrentItem currentItem,
                                  [NotNull] ICopyMachine copyMachine, [NotNull] ICopyProgress copyProgress)
        {
            _currentItem = currentItem ?? throw new ArgumentNullException(nameof(currentItem));
            _copyMachine = copyMachine ?? throw new ArgumentNullException(nameof(copyMachine));
            _copyProgress = copyProgress ?? throw new ArgumentNullException(nameof(copyProgress));
            _dialogCoordinator = instance ?? throw new ArgumentNullException(nameof(instance));
            _reloadDefaultCommand = reloadDefaultCommand ?? throw new ArgumentNullException(nameof(reloadDefaultCommand));
        }

        /// <inheritdoc />
        public async Task RunAsync()
        {
            var result = await _dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "Copy machine...",
                $"Are you sure you want to copy machine '{_currentItem.Value.DisplayName}'?",
                MessageDialogStyle.AffirmativeAndNegative).ConfigureAwait(true);

            if (result == MessageDialogResult.Affirmative)
            {
                try
                {
                    var inputResult = await _dialogCoordinator.ShowInputAsync(DialogCoordinatorContext, "Copy machine...", "Enter the new directory name").ConfigureAwait(true);

                    if (inputResult != null)
                    {
                        var controller = await _dialogCoordinator.ShowProgressAsync(DialogCoordinatorContext, "Copy machine...", "Please wait until the process has finished");
                        controller.Maximum = 100d;

                        await Task.Run(async () =>
                                       {
                                           _copyProgress.Progress = new Progress<double>(increment =>
                                                                                         {
                                                                                             controller.SetProgress(increment);
                                                                                             if (increment.Equals(100d))
                                                                                             {
                                                                                                 controller.SetMessage("Finished process");
                                                                                             }
                                                                                         });

                                           await _copyMachine.RunForAsync(_currentItem.Value, inputResult);
                                       });
                        await controller.CloseAsync();
                    }
                }
                catch (IOException ioException)
                {
                    await _dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "'Copy machine' was canceled", ioException.Message);
                }
                catch (Exception exception)
                {
                    await _dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "'Copy machine' was canceled", exception.Message);
                }

                await _reloadDefaultCommand.RunAsync();
            }
        }

        /// <inheritdoc />
        public DefaultCommand Value
        {
            get
            {
                async void Execute(object _) => await RunAsync();

                return new()
                       {
                           Command = new RelayCommand(Execute)
                       };
            }
        }

        /// <inheritdoc />
        public object DialogCoordinatorContext { get; set; }
    }
}