using System.IO;
using EvilBaschdi.Core.Internal;
using MahApps.Metro.Controls.Dialogs;

namespace VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

/// <inheritdoc />
public class CopyDefaultCommand(
    [NotNull] IDialogCoordinator instance,
    [NotNull] IReloadDefaultCommand reloadDefaultCommand,
    [NotNull] ICurrentMachine currentMachine,
    [NotNull] ICopyMachine copyMachine,
    [NotNull] ICopyProgress copyProgress) : ICopyDefaultCommand
{
    private readonly ICopyMachine _copyMachine = copyMachine ?? throw new ArgumentNullException(nameof(copyMachine));
    private readonly ICopyProgress _copyProgress = copyProgress ?? throw new ArgumentNullException(nameof(copyProgress));
    private readonly ICurrentMachine _currentMachine = currentMachine ?? throw new ArgumentNullException(nameof(currentMachine));
    private readonly IDialogCoordinator _dialogCoordinator = instance ?? throw new ArgumentNullException(nameof(instance));
    private readonly IReloadDefaultCommand _reloadDefaultCommand = reloadDefaultCommand ?? throw new ArgumentNullException(nameof(reloadDefaultCommand));

    /// <inheritdoc />
    public async Task Value()
    {
        var machine = _currentMachine.Value;
        if (!machine.IsEnabledForEditing)
        {
            await _dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "'Copy machine' was canceled", "Machine is currently read only");
            return;
        }

        var result = await _dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "Copy machine...",
            $"Are you sure you want to copy machine '{machine.DisplayName}'?",
            MessageDialogStyle.AffirmativeAndNegative).ConfigureAwait(true);

        if (result == MessageDialogResult.Affirmative)
        {
            try
            {
                var inputResult = await _dialogCoordinator.ShowInputAsync(
                    DialogCoordinatorContext,
                    "Copy machine...",
                    "Enter the new directory name",
                    new()
                    {
                        DefaultText = machine.DisplayName,
                        ColorScheme = MetroDialogColorScheme.Accented
                    }
                );

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

                                       await _copyMachine.ValueFor(machine, inputResult);
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

            await _reloadDefaultCommand.Value();
        }
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