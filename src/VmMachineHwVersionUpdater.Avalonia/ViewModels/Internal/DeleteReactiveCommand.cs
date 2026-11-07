using FluentAvalonia.UI.Controls;
using VmMachineHwVersionUpdater.Avalonia.ViewModels;

//using FluentAvalonia.UI.Controls;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IDeleteReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandRxVoidTask" />
public class DeleteReactiveCommand(
    [NotNull] IDeleteMachine deleteMachine,
    [NotNull] ICurrentMachine currentMachine,
    [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime) : ReactiveCommandRxVoidTask, IDeleteReactiveCommand
{
    private readonly IDeleteMachine _deleteMachine = deleteMachine ?? throw new ArgumentNullException(nameof(deleteMachine));
    private readonly ICurrentMachine _currentMachine = currentMachine ?? throw new ArgumentNullException(nameof(currentMachine));

    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime =
        mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));

    /// <inheritdoc />
    public override async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var mainWindow = _mainWindowByApplicationLifetime.Value;

        if (mainWindow is not null)
        {
            var title = "Delete machine...";
            var text = $"Are you sure you want to delete machine '{_currentMachine.Value.DisplayName}'?";

            var confirmationDialog = new FAContentDialog
                                     {
                                         Title = title,
                                         Content = text,
                                         PrimaryButtonText = "Yes",
                                         SecondaryButtonText = "No"
                                     };
            var result = await confirmationDialog.ShowAsync();

            var exceptionDialog = new FATaskDialog
                                  {
                                      Title = "'Delete machine' was canceled",
                                      IconSource = new FASymbolIconSource { Symbol = FASymbol.AlertUrgentFilled },
                                      Buttons =
                                      {
                                          FATaskDialogButton.OKButton,
                                      },
                                      XamlRoot = mainWindow
                                  };

            if (result != FAContentDialogResult.Primary)
            {
                return;
            }

            try
            {
                _deleteMachine.RunFor(_currentMachine.Value.Path);

                if (mainWindow.DataContext is not IMainWindowViewModel mainWindowViewModel)
                {
                    throw new InvalidOperationException("The main window view model is not available.");
                }

                await mainWindowViewModel.RefreshMachineDataAsync();
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