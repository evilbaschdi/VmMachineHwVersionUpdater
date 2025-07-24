using EvilBaschdi.Core.Avalonia;
using FluentAvalonia.UI.Controls;
using VmMachineHwVersionUpdater.Core.Commands;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IStartReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitTask" />
public class StartReactiveCommand(
    [NotNull] IStartCommand startCommand,
    [NotNull] ICurrentMachine currentMachine,
    [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime)
    : ReactiveCommandUnitTask, IStartReactiveCommand
{
    private readonly IStartCommand _startCommand = startCommand ?? throw new ArgumentNullException(nameof(startCommand));
    private readonly ICurrentMachine _currentMachine = currentMachine ?? throw new ArgumentNullException(nameof(currentMachine));

    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime =
        mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));

    /// <summary>
    ///     Starts VM
    /// </summary>
    public override async Task RunAsync()
    {
        var mainWindow = _mainWindowByApplicationLifetime.Value;
        var currentMachine = _currentMachine.Value;

        if (mainWindow != null && currentMachine == null)
        {
            var exceptionDialog = new TaskDialog
                                  {
                                      Title = "No row selected!",
                                      IconSource = new SymbolIconSource { Symbol = Symbol.AlertUrgentFilled },
                                      Buttons =
                                      {
                                          TaskDialogButton.OKButton,
                                      },
                                      XamlRoot = mainWindow,
                                      Content = "Please select a machine to start"
                                  };

            await exceptionDialog.ShowAsync();
        }

        await Task.Run(_startCommand.Run);
    }
}
