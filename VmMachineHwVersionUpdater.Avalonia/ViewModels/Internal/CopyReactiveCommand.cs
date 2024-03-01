using Avalonia.Threading;
using EvilBaschdi.Core.Avalonia;
using EvilBaschdi.Core.Avalonia.Controls;
using EvilBaschdi.Core.Internal;
using FluentAvalonia.UI.Controls;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="ICopyReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
/// <summary>
///     Constructor
/// </summary>
/// <param name="copyMachine"></param>
/// <param name="copyProgress"></param>
/// <param name="currentItem"></param>
/// <param name="reloadReactiveCommand"></param>
/// <param name="mainWindowByApplicationLifetime"></param>
/// <exception cref="ArgumentNullException"></exception>
public class CopyReactiveCommand(
    [NotNull] ICopyMachine copyMachine,
    [NotNull] ICopyProgress copyProgress,
    [NotNull] ICurrentItem currentItem,
    [NotNull] IReloadReactiveCommand reloadReactiveCommand,
    [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime) : ReactiveCommandUnitRun, ICopyReactiveCommand
{
    private readonly ICopyMachine _copyMachine = copyMachine ?? throw new ArgumentNullException(nameof(copyMachine));
    private readonly ICopyProgress _copyProgress = copyProgress ?? throw new ArgumentNullException(nameof(copyProgress));
    private readonly ICurrentItem _currentItem = currentItem ?? throw new ArgumentNullException(nameof(currentItem));
    private readonly IReloadReactiveCommand _reloadReactiveCommand = reloadReactiveCommand ?? throw new ArgumentNullException(nameof(reloadReactiveCommand));

    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime =
        mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));

    /// <inheritdoc />
    public override async void Run()
    {
        var mainWindow = _mainWindowByApplicationLifetime.Value;
        var machine = _currentItem.Value;

        if (!machine.IsEnabledForEditing)
        {
            var readOnlyDialog = new TaskDialog
                                 {
                                     Buttons =
                                     {
                                         TaskDialogButton.OKButton,
                                     },
                                     XamlRoot = mainWindow,
                                     Title = "'Copy machine' was canceled",
                                     IconSource = new SymbolIconSource { Symbol = Symbol.AlertUrgentFilled },
                                     Content = "Machine is currently read only"
                                 };
            await readOnlyDialog.ShowAsync();
            return;
        }

        var title = "Copy machine...";
        var text = $"Are you sure you want to copy machine '{_currentItem.Value.DisplayName}'?";

        var confirmationDialog = new ContentDialog
                                 {
                                     Title = title,
                                     Content = text,
                                     PrimaryButtonText = "Yes",
                                     SecondaryButtonText = "No"
                                 };
        var confirmationResult = await confirmationDialog.ShowAsync();

        if (confirmationResult != ContentDialogResult.Primary)
        {
            return;
        }

        var exceptionDialog = new TaskDialog
                              {
                                  Title = "'Copy machine' was canceled",
                                  IconSource = new SymbolIconSource { Symbol = Symbol.AlertUrgentFilled },
                                  Buttons =
                                  {
                                      TaskDialogButton.OKButton,
                                  },
                                  XamlRoot = mainWindow
                              };

        try
        {
            var targetDialog = new ContentDialog
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

            if (targetDialogResult == ContentDialogResult.Primary && !string.IsNullOrWhiteSpace(input.ResultText))
            {
                var copyDialog = new TaskDialog
                                 {
                                     Title = title,
                                     ShowProgressBar = true,
                                     IconSource = new SymbolIconSource { Symbol = Symbol.CopyFilled },
                                     SubHeader = "Copying",
                                     Content = "Please wait while the maching gets copied",
                                     Buttons =
                                     {
                                         TaskDialogButton.CancelButton
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
                                                                                                                  TaskDialogProgressState.Normal);
                                                                                                          });

                                                            await _copyMachine.ValueFor(machine, input.ResultText);

                                                            // All done, auto close the dialog here
                                                            Dispatcher.UIThread.Post(() => { copyDialog.Hide(TaskDialogStandardResult.OK); });
                                                        });
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

        _reloadReactiveCommand.Run();
    }
}