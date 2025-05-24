using EvilBaschdi.Core.Avalonia;
using FluentAvalonia.UI.Controls;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IArchiveReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitTask" />
public class ArchiveReactiveCommand(
    [NotNull] IArchiveMachine archiveMachine,
    [NotNull] ICurrentMachine currentMachine,
    [NotNull] IReloadReactiveCommand reloadReactiveCommand,
    [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime) : ReactiveCommandUnitTask, IArchiveReactiveCommand
{
    private readonly IArchiveMachine _archiveMachine = archiveMachine ?? throw new ArgumentNullException(nameof(archiveMachine));
    private readonly ICurrentMachine _currentMachine = currentMachine ?? throw new ArgumentNullException(nameof(currentMachine));
    private readonly IReloadReactiveCommand _reloadReactiveCommand = reloadReactiveCommand ?? throw new ArgumentNullException(nameof(reloadReactiveCommand));

    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime =
        mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));

    /// <inheritdoc />
    public override async Task RunAsync()
    {
        var mainWindow = _mainWindowByApplicationLifetime.Value;

        if (mainWindow != null)
        {
            var title = "Archive machine...";
            var text = $"Are you sure you want to archive machine '{_currentMachine.Value.DisplayName}'?";

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
                                      Title = "'Archive machine' was canceled",
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
                _archiveMachine.RunFor(_currentMachine.Value);

                await _reloadReactiveCommand.RunAsync();
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