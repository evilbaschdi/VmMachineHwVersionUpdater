using System.IO;
using EvilBaschdi.Core.Internal;
using MahApps.Metro.Controls.Dialogs;

namespace VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

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
    public async Task Value()
    {
        var machine = _currentItem.Value;
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
            async void Execute(object _) => await Value();

            return new()
                   {
                       Command = new RelayCommand(Execute)
                   };
        }
    }

    /// <inheritdoc />
    public object DialogCoordinatorContext { get; set; }
}