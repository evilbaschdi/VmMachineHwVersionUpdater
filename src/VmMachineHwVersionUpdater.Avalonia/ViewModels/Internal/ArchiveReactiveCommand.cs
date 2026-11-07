using FluentAvalonia.UI.Controls;
using VmMachineHwVersionUpdater.Avalonia.ViewModels;

//using FluentAvalonia.UI.Controls;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IArchiveReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandRxVoidTask" />
public class ArchiveReactiveCommand(
    [NotNull] IArchiveMachine archiveMachine,
    [NotNull] ICurrentMachine currentMachine,
    [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime) : ReactiveCommandRxVoidTask, IArchiveReactiveCommand
{
    private readonly IArchiveMachine _archiveMachine = archiveMachine ?? throw new ArgumentNullException(nameof(archiveMachine));
    private readonly ICurrentMachine _currentMachine = currentMachine ?? throw new ArgumentNullException(nameof(currentMachine));

    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime =
        mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));

    /// <inheritdoc />
    public override async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var mainWindow = _mainWindowByApplicationLifetime.Value;

        if (mainWindow is not null)
        {
            var title = "Archive machine...";
            var text = $"Are you sure you want to archive machine '{_currentMachine.Value.DisplayName}'?";

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
                                      Title = "'Archive machine' was canceled",
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
                _archiveMachine.RunFor(_currentMachine.Value);

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