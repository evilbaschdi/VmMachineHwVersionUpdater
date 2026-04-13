using FluentAvalonia.UI.Controls;

//using FluentAvalonia.UI.Controls;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IRenameReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitTask" />
public class RenameReactiveCommand(
    [NotNull] IChangeDisplayName changeDisplayName,
    [NotNull] ICurrentMachine currentMachine,
    [NotNull] IReloadReactiveCommand reloadReactiveCommand,
    [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime
) : ReactiveCommandUnitTask, IRenameReactiveCommand
{
    private readonly ICurrentMachine _currentMachine = currentMachine ?? throw new ArgumentNullException(nameof(currentMachine));
    private readonly IReloadReactiveCommand _reloadReactiveCommand = reloadReactiveCommand ?? throw new ArgumentNullException(nameof(reloadReactiveCommand));

    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime =
        mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));

    private readonly IChangeDisplayName _changeDisplayName = changeDisplayName ?? throw new ArgumentNullException(nameof(changeDisplayName));

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
                                     Title = "'Rename machine' was canceled",
                                     IconSource = new FASymbolIconSource { Symbol = FASymbol.AlertUrgentFilled },
                                     Content = "Machine is currently read only"
                                 };
            await readOnlyDialog.ShowAsync();
            return;
        }

        var title = "Rename machine...";
        var text = $"Are you sure you want to rename machine '{_currentMachine.Value.DisplayName}'?";

        var confirmationDialog = new FAContentDialog
                                 {
                                     Title = title,
                                     Content = text,
                                     PrimaryButtonText = "Yes",
                                     SecondaryButtonText = "No"
                                 };
        var result = await confirmationDialog.ShowAsync();

        if (result != FAContentDialogResult.Primary)
        {
            return;
        }

        var exceptionDialog = new FATaskDialog
                              {
                                  Title = "'Rename machine' was canceled",
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
                                   Title = "Enter the new display name",
                                   PrimaryButtonText = "Ok",
                                   CloseButtonText = "Cancel"
                               };

            var input = new ContentDialogInput
                        {
                            CaptionText = "Enter the new display name"
                        };
            targetDialog.Content = input;

            var targetDialogResult = await targetDialog.ShowAsync();

            if (targetDialogResult == FAContentDialogResult.Primary && !string.IsNullOrWhiteSpace(input.ResultText))
            {
                _changeDisplayName.RunFor(machine.Path, input.ResultText);
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