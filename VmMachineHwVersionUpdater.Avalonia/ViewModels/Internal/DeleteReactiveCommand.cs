using EvilBaschdi.Core.Avalonia;
using FluentAvalonia.UI.Controls;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IDeleteReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
/// <summary>
///     Constructor
/// </summary>
/// <param name="deleteMachine"></param>
/// <param name="currentItem"></param>
/// <param name="reloadReactiveCommand"></param>
/// <param name="mainWindowByApplicationLifetime"></param>
/// <exception cref="ArgumentNullException"></exception>
public class DeleteReactiveCommand(
    [NotNull] IDeleteMachine deleteMachine,
    [NotNull] ICurrentItem currentItem,
    [NotNull] IReloadReactiveCommand reloadReactiveCommand,
    [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime) : ReactiveCommandUnitRun, IDeleteReactiveCommand
{
    private readonly IDeleteMachine _deleteMachine = deleteMachine ?? throw new ArgumentNullException(nameof(deleteMachine));
    private readonly ICurrentItem _currentItem = currentItem ?? throw new ArgumentNullException(nameof(currentItem));
    private readonly IReloadReactiveCommand _reloadReactiveCommand = reloadReactiveCommand ?? throw new ArgumentNullException(nameof(reloadReactiveCommand));

    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime =
        mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));

    /// <inheritdoc />
    public override async void Run()
    {
        var mainWindow = _mainWindowByApplicationLifetime.Value;

        if (mainWindow != null)
        {
            var title = "Delete machine...";
            var text = $"Are you sure you want to delete machine '{_currentItem.Value.DisplayName}'?";

            var confirmationDialog = new ContentDialog
                                     {
                                         Title = title,
                                         Content = text,
                                         PrimaryButtonText = "Yes",
                                         SecondaryButtonText = "No"
                                     };
            var result = await confirmationDialog.ShowAsync();

            var exceptionDialog = new TaskDialog
                                  {
                                      Title = "'Delete machine' was canceled",
                                      IconSource = new SymbolIconSource { Symbol = Symbol.AlertUrgentFilled },
                                      Buttons =
                                      {
                                          TaskDialogButton.OKButton,
                                      },
                                      XamlRoot = mainWindow
                                  };

            if (result != ContentDialogResult.Primary)
            {
                return;
            }

            try
            {
                _deleteMachine.RunFor(_currentItem.Value.Path);

                _reloadReactiveCommand.Run();
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
        }
    }
}