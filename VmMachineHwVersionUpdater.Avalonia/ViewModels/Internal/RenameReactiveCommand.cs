using EvilBaschdi.Core.Avalonia;
using EvilBaschdi.Core.Avalonia.Controls;
using FluentAvalonia.UI.Controls;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IRenameReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
/// <summary>
///     Constructor
/// </summary>
/// <param name="changeDisplayName"></param>
/// <param name="currentMachine"></param>
/// <param name="reloadReactiveCommand"></param>
/// <param name="mainWindowByApplicationLifetime"></param>
/// <exception cref="ArgumentNullException"></exception>
public class RenameReactiveCommand(
    [NotNull] IChangeDisplayName changeDisplayName,
    [NotNull] ICurrentMachine currentMachine,
    [NotNull] IReloadReactiveCommand reloadReactiveCommand,
    [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime
) : ReactiveCommandUnitRun, IRenameReactiveCommand
{
    private readonly ICurrentMachine _currentMachine = currentMachine ?? throw new ArgumentNullException(nameof(currentMachine));
    private readonly IReloadReactiveCommand _reloadReactiveCommand = reloadReactiveCommand ?? throw new ArgumentNullException(nameof(reloadReactiveCommand));

    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime =
        mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));

    private readonly IChangeDisplayName _changeDisplayName = changeDisplayName ?? throw new ArgumentNullException(nameof(changeDisplayName));

    /// <inheritdoc />
    public override async void Run()
    {
        var mainWindow = _mainWindowByApplicationLifetime.Value;
        var machine = _currentMachine.Value;

        if (!machine.IsEnabledForEditing)
        {
            var readOnlyDialog = new TaskDialog
                                 {
                                     Buttons =
                                     {
                                         TaskDialogButton.OKButton,
                                     },
                                     XamlRoot = mainWindow,
                                     Title = "'Rename machine' was canceled",
                                     IconSource = new SymbolIconSource { Symbol = Symbol.AlertUrgentFilled },
                                     Content = "Machine is currently read only"
                                 };
            await readOnlyDialog.ShowAsync();
            return;
        }

        var title = "Rename machine...";
        var text = $"Are you sure you want to rename machine '{_currentMachine.Value.DisplayName}'?";

        var confirmationDialog = new ContentDialog
                                 {
                                     Title = title,
                                     Content = text,
                                     PrimaryButtonText = "Yes",
                                     SecondaryButtonText = "No"
                                 };
        var result = await confirmationDialog.ShowAsync();

        if (result != ContentDialogResult.Primary)
        {
            return;
        }

        var exceptionDialog = new TaskDialog
                              {
                                  Title = "'Rename machine' was canceled",
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

            if (targetDialogResult == ContentDialogResult.Primary && !string.IsNullOrWhiteSpace(input.ResultText))
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

        _reloadReactiveCommand.Run();
    }
}