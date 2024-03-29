﻿using EvilBaschdi.Core.Avalonia;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IArchiveReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
/// <summary>
///     Constructor
/// </summary>
/// <param name="archiveMachine"></param>
/// <param name="currentItem"></param>
/// <param name="reloadReactiveCommand"></param>
/// <param name="mainWindowByApplicationLifetime"></param>
/// <exception cref="ArgumentNullException"></exception>
public class ArchiveReactiveCommand([NotNull] IArchiveMachine archiveMachine,
                              [NotNull] ICurrentItem currentItem,
                              [NotNull] IReloadReactiveCommand reloadReactiveCommand,
                              [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime) : ReactiveCommandUnitRun, IArchiveReactiveCommand
{
    [NotNull] private readonly IArchiveMachine _archiveMachine = archiveMachine ?? throw new ArgumentNullException(nameof(archiveMachine));
    [NotNull] private readonly ICurrentItem _currentItem = currentItem ?? throw new ArgumentNullException(nameof(currentItem));
    [NotNull] private readonly IReloadReactiveCommand _reloadReactiveCommand = reloadReactiveCommand ?? throw new ArgumentNullException(nameof(reloadReactiveCommand));
    [NotNull] private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime = mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));

    /// <inheritdoc />
    public override async void Run()
    {
        var mainWindow = _mainWindowByApplicationLifetime.Value;

        if (mainWindow != null)
        {
            var title = "Archive machine...";
            var text = $"Are you sure you want to archive '{_currentItem.Value.DisplayName}'?";

            var box = MessageBoxManager.GetMessageBoxStandard(title, text, ButtonEnum.YesNo, Icon.Question);
            var result = await box.ShowAsPopupAsync(mainWindow);

            if (result == ButtonResult.Yes)
            {
                try
                {
                    _archiveMachine.RunFor(_currentItem.Value);

                    _reloadReactiveCommand.Run();
                }
                catch (IOException ioException)
                {
                    var ioExceptionBox = MessageBoxManager.GetMessageBoxStandard(ioException.Message, "'Archive machine' was canceled", ButtonEnum.Ok, Icon.Error);
                    await ioExceptionBox.ShowAsPopupAsync(mainWindow);
                }
                catch (Exception exception)
                {
                    var exceptionBox = MessageBoxManager.GetMessageBoxStandard(exception.Message, "'Archive machine' was canceled", ButtonEnum.Ok, Icon.Error);
                    await exceptionBox.ShowAsPopupAsync(mainWindow);
                }
            }
        }
    }
}