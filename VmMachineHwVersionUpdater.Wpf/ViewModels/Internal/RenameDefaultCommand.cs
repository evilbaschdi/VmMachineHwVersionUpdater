﻿using System.IO;
using MahApps.Metro.Controls.Dialogs;

namespace VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

/// <inheritdoc />
public class RenameDefaultCommand(
    [NotNull] IDialogCoordinator instance,
    [NotNull] IReloadDefaultCommand reloadDefaultCommand,
    [NotNull] ICurrentMachine currentMachine,
    [NotNull] IChangeDisplayName changeDisplayName) : IRenameDefaultCommand
{
    private readonly IChangeDisplayName _changeDisplayName = changeDisplayName ?? throw new ArgumentNullException(nameof(changeDisplayName));
    private readonly ICurrentMachine _currentMachine = currentMachine ?? throw new ArgumentNullException(nameof(currentMachine));
    private readonly IDialogCoordinator _dialogCoordinator = instance ?? throw new ArgumentNullException(nameof(instance));
    private readonly IReloadDefaultCommand _reloadDefaultCommand = reloadDefaultCommand ?? throw new ArgumentNullException(nameof(reloadDefaultCommand));

    /// <inheritdoc />
    public async Task Value()
    {
        var machine = _currentMachine.Value;

        if (!machine.IsEnabledForEditing)
        {
            await _dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "'Rename machine' was canceled", "Machine is currently read only");
            return;
        }

        var result = await _dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "Rename machine...",
            $"Are you sure you want to rename machine '{machine.DisplayName}'?",
            MessageDialogStyle.AffirmativeAndNegative).ConfigureAwait(true);

        if (result == MessageDialogResult.Affirmative)
        {
            try
            {
                var inputResult = await _dialogCoordinator.ShowInputAsync(
                    DialogCoordinatorContext,
                    "Rename machine...",
                    "Enter the new display name",
                    new()
                    {
                        DefaultText = machine.DisplayName,
                        ColorScheme = MetroDialogColorScheme.Accented
                    }
                ).ConfigureAwait(true);

                if (inputResult != null)
                {
                    var displayNameToSet = inputResult.Trim();
                    if (!string.IsNullOrWhiteSpace(displayNameToSet))
                    {
                        var controller = await _dialogCoordinator.ShowProgressAsync(DialogCoordinatorContext, "Rename machine...", "Please wait until the process has finished");
                        controller.Maximum = 100d;

                        await Task.Run(() => { _changeDisplayName.RunFor(machine.Path, displayNameToSet); });
                        await controller.CloseAsync();
                    }
                }
            }
            catch (IOException ioException)
            {
                await _dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "'Rename machine' was canceled", ioException.Message);
            }
            catch (Exception exception)
            {
                await _dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "'Rename machine' was canceled", exception.Message);
            }
        }

        await _reloadDefaultCommand.Value();
    }

    /// <inheritdoc />
    public DefaultCommand DefaultCommandValue
    {
        get
        {
            return new()
                   {
                       Command = new RelayCommand(Execute)
                   };

            // ReSharper disable once AsyncVoidMethod
            async void Execute(object _) => await Value();
        }
    }

    /// <inheritdoc />
    public object DialogCoordinatorContext { get; set; }
}