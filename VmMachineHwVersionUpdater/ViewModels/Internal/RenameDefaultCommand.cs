using System.IO;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using JetBrains.Annotations;
using MahApps.Metro.Controls.Dialogs;
using VmMachineHwVersionUpdater.Core.Models;
using VmMachineHwVersionUpdater.Core.PerMachine;

namespace VmMachineHwVersionUpdater.ViewModels.Internal;

/// <inheritdoc />
public class RenameDefaultCommand : IRenameDefaultCommand
{
    private readonly IChangeDisplayName _changeDisplayName;
    private readonly ICurrentItem _currentItem;
    private readonly IDialogCoordinator _dialogCoordinator;
    private readonly IReloadDefaultCommand _reloadDefaultCommand;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="reloadDefaultCommand"></param>
    /// <param name="currentItem"></param>
    /// <param name="instance"></param>
    /// <param name="changeDisplayName"></param>
    public RenameDefaultCommand([NotNull] IDialogCoordinator instance, [NotNull] IReloadDefaultCommand reloadDefaultCommand, [NotNull] ICurrentItem currentItem,
                                [NotNull] IChangeDisplayName changeDisplayName)
    {
        _currentItem = currentItem ?? throw new ArgumentNullException(nameof(currentItem));
        _changeDisplayName = changeDisplayName ?? throw new ArgumentNullException(nameof(changeDisplayName));
        _dialogCoordinator = instance ?? throw new ArgumentNullException(nameof(instance));
        _reloadDefaultCommand = reloadDefaultCommand ?? throw new ArgumentNullException(nameof(reloadDefaultCommand));
    }

    /// <inheritdoc />
    public async Task RunAsync()
    {
        var machine = _currentItem.Value;

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

        await _reloadDefaultCommand.RunAsync();
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