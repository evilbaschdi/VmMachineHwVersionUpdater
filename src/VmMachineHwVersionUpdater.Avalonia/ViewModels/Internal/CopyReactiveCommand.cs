using Avalonia.Threading;
using EvilBaschdi.Core.Internal.Copy;
using FluentAvalonia.UI.Controls;

//using FluentAvalonia.UI.Controls;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="ICopyReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitTask" />
public class CopyReactiveCommand(
    [NotNull] ICopyMachine copyMachine,
    [NotNull] ICopyProgress copyProgress,
    [NotNull] ICurrentMachine currentMachine,
    [NotNull] IReloadReactiveCommand reloadReactiveCommand,
    [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime) : ReactiveCommandUnitTask, ICopyReactiveCommand
{
    private readonly ICopyMachine _copyMachine = copyMachine ?? throw new ArgumentNullException(nameof(copyMachine));
    private readonly ICopyProgress _copyProgress = copyProgress ?? throw new ArgumentNullException(nameof(copyProgress));
    private readonly ICurrentMachine _currentMachine = currentMachine ?? throw new ArgumentNullException(nameof(currentMachine));
    private readonly IReloadReactiveCommand _reloadReactiveCommand = reloadReactiveCommand ?? throw new ArgumentNullException(nameof(reloadReactiveCommand));

    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime =
        mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));

    /// <inheritdoc />
    public override async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var mainWindow = _mainWindowByApplicationLifetime.Value;
        var machine = _currentMachine.Value;

        if (!machine.IsEnabledForEditing)
        {
            var readOnlyDialog = new FATaskDialog
                                 {
                                     Buttons =
                                     {
                                         FATaskDialogButton.OKButton,
                                     },
                                     XamlRoot = mainWindow,
                                     Title = "'Copy machine' was canceled",
                                     IconSource = new FASymbolIconSource { Symbol = FASymbol.AlertUrgentFilled },
                                     Content = "Machine is currently read only"
                                 };
            await readOnlyDialog.ShowAsync();
            return;
        }

        var title = "Copy machine...";
        var text = $"Are you sure you want to copy machine '{_currentMachine.Value.DisplayName}'?";

        var confirmationDialog = new FAContentDialog
                                 {
                                     Title = title,
                                     Content = text,
                                     PrimaryButtonText = "Yes",
                                     SecondaryButtonText = "No"
                                 };
        var confirmationResult = await confirmationDialog.ShowAsync();

        if (confirmationResult != FAContentDialogResult.Primary)
        {
            return;
        }

        var exceptionDialog = new FATaskDialog
                              {
                                  Title = "'Copy machine' was canceled",
                                  IconSource = new FASymbolIconSource { Symbol = FASymbol.AlertUrgentFilled },
                                  Buttons =
                                  {
                                      FATaskDialogButton.OKButton,
                                  },
                                  XamlRoot = mainWindow
                              };

        try
        {
            var targetDialog = new FAContentDialog
                               {
                                   Title = "Enter the new directory name",
                                   PrimaryButtonText = "Ok",
                                   CloseButtonText = "Cancel"
                               };

            var input = new ContentDialogInput
                        {
                            CaptionText = "Enter the new directory name"
                        };
            targetDialog.Content = input;

            var targetDialogResult = await targetDialog.ShowAsync();

            if (targetDialogResult == FAContentDialogResult.Primary && !string.IsNullOrWhiteSpace(input.ResultText))
            {
                var copyDialog = new FATaskDialog
                                 {
                                     Title = title,
                                     ShowProgressBar = true,
                                     IconSource = new FASymbolIconSource { Symbol = FASymbol.CopyFilled },
                                     SubHeader = "Copying",
                                     Content = "Please wait while the machine gets copied",
                                     Buttons =
                                     {
                                         FATaskDialogButton.CancelButton
                                     }
                                 };

                copyDialog.Opened += async (_, _) =>
                                     {
                                         // We immediately begin the progress task as soon as the dialog opens

                                         // NOTE: Cancelling the dialog while this is running won't stop this this async process, you should
                                         //       use a CancellationToken and monitor the dialog result to cancel this task
                                         await Task.Run(async () =>
                                                        {
                                                            _copyProgress.Progress = new Progress<double>(progress =>
                                                                                                          {
                                                                                                              copyDialog.SetProgressBarState(progress,
                                                                                                                  FATaskDialogProgressState.Normal);
                                                                                                          });

                                                            await _copyMachine.RunForAsync(machine, input.ResultText, cancellationToken);

                                                            // All done, auto close the dialog here
                                                            Dispatcher.UIThread.Post(() => { copyDialog.Hide(FATaskDialogStandardResult.OK); });
                                                        }, cancellationToken);
                                     };

                copyDialog.XamlRoot = mainWindow;
                await copyDialog.ShowAsync();
            }
        }
        catch (IOException ioException)
        {
            exceptionDialog.Content = ioException.Message;
            await exceptionDialog.ShowAsync();
        }
        catch (Exception exception)
        {
            exceptionDialog.Content = exception.Message;
            await exceptionDialog.ShowAsync();
        }

        await _reloadReactiveCommand.RunAsync(cancellationToken);
    }
}