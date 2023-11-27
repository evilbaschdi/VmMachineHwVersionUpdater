﻿using EvilBaschdi.Core.Avalonia;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IDeleteReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
public class DeleteReactiveCommand : ReactiveCommandUnitRun, IDeleteReactiveCommand
{
    private readonly IDeleteMachine _deleteMachine;
    private readonly ICurrentItem _currentItem;
    private readonly IReloadReactiveCommand _reloadReactiveCommand;
    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="deleteMachine"></param>
    /// <param name="currentItem"></param>
    /// <param name="reloadReactiveCommand"></param>
    /// <param name="mainWindowByApplicationLifetime"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public DeleteReactiveCommand([NotNull] IDeleteMachine deleteMachine,
                                 [NotNull] ICurrentItem currentItem,
                                 [NotNull] IReloadReactiveCommand reloadReactiveCommand,
                                 [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime)
    {
        _deleteMachine = deleteMachine ?? throw new ArgumentNullException(nameof(deleteMachine));
        _currentItem = currentItem ?? throw new ArgumentNullException(nameof(currentItem));
        _reloadReactiveCommand = reloadReactiveCommand ?? throw new ArgumentNullException(nameof(reloadReactiveCommand));
        _mainWindowByApplicationLifetime = mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));
    }

    /// <inheritdoc />
    public override async void Run()
    {
        var mainWindow = _mainWindowByApplicationLifetime.Value;

        if (mainWindow != null)
        {
            var title = "Delete machine...";
            var text = $"Are you sure you want to delete '{_currentItem.Value.DisplayName}'?";

            var box = MessageBoxManager.GetMessageBoxStandard(title, text, ButtonEnum.YesNo, Icon.Question);
            var result = await box.ShowAsPopupAsync(mainWindow);

            if (result == ButtonResult.Yes)
            {
                try
                {
                    _deleteMachine.RunFor(_currentItem.Value.Path);

                    _reloadReactiveCommand.Run();
                }
                catch (IOException ioException)
                {
                    var ioExceptionBox = MessageBoxManager.GetMessageBoxStandard(ioException.Message, "'Delete machine' was canceled", ButtonEnum.Ok, Icon.Error);
                    await ioExceptionBox.ShowAsPopupAsync(mainWindow);
                }
                catch (Exception exception)
                {
                    var exceptionBox = MessageBoxManager.GetMessageBoxStandard(exception.Message, "'Delete machine' was canceled", ButtonEnum.Ok, Icon.Error);
                    await exceptionBox.ShowAsPopupAsync(mainWindow);
                }
            }
        }
    }
}