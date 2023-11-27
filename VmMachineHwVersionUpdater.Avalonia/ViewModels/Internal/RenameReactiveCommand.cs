using Avalonia.Controls;
using EvilBaschdi.Core.Avalonia;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IRenameReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
public class RenameReactiveCommand : ReactiveCommandUnitRun, IRenameReactiveCommand
{
    private readonly ICurrentItem _currentItem;
    private readonly IReloadReactiveCommand _reloadReactiveCommand;
    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime;
    private readonly IChangeDisplayName _changeDisplayName;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="changeDisplayName"></param>
    /// <param name="currentItem"></param>
    /// <param name="reloadReactiveCommand"></param>
    /// <param name="mainWindowByApplicationLifetime"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public RenameReactiveCommand([NotNull] IChangeDisplayName changeDisplayName,
                                 [NotNull] ICurrentItem currentItem,
                                 [NotNull] IReloadReactiveCommand reloadReactiveCommand,
                                 [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime
    )
    {
        _currentItem = currentItem ?? throw new ArgumentNullException(nameof(currentItem));
        _reloadReactiveCommand = reloadReactiveCommand ?? throw new ArgumentNullException(nameof(reloadReactiveCommand));
        _mainWindowByApplicationLifetime = mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));
        _changeDisplayName = changeDisplayName ?? throw new ArgumentNullException(nameof(changeDisplayName));
    }

    /// <inheritdoc />
    public override async void Run()
    {
        var mainWindow = _mainWindowByApplicationLifetime.Value;
        var machine = _currentItem.Value;
        if (!machine.IsEnabledForEditing)
        {
            var warningBox = MessageBoxManager.GetMessageBoxStandard("'Rename machine' was canceled", "Machine is currently read only", ButtonEnum.Ok, Icon.Warning);
            await warningBox.ShowAsPopupAsync(mainWindow);
            return;
        }

        var box = MessageBoxManager.GetMessageBoxStandard("Rename machine...", $"Are you sure you want to rename machine '{machine.DisplayName}'?", ButtonEnum.YesNo,
            Icon.Question);
        var result = await box.ShowAsPopupAsync(mainWindow);

        if (result == ButtonResult.Yes)
        {
            try
            {
                var inputBox = MessageBoxManager.GetMessageBoxStandard(
                    new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.OkCancel,
                        ContentTitle = "Rename machine...",
                        //ContentHeader = "Copy machine...",
                        ContentMessage = "Enter the new display name",
                        Icon = Icon.Setting,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner,
                        CanResize = false,
                        SizeToContent = SizeToContent.WidthAndHeight,
                        ShowInCenter = true,
                        Topmost = false,
                        InputParams = new InputParams
                                      {
                                          Multiline = false
                                      }
                    });

                var inputResult = await inputBox.ShowAsPopupAsync(mainWindow);

                if (inputResult == ButtonResult.Ok && !string.IsNullOrWhiteSpace(inputBox.InputValue))
                {
                    _changeDisplayName.RunFor(machine.Path, inputBox.InputValue);
                }
            }
            catch (IOException ioException)
            {
                var ioExceptionBox = MessageBoxManager.GetMessageBoxStandard(ioException.Message, "'Rename machine' was canceled", ButtonEnum.Ok, Icon.Error);
                await ioExceptionBox.ShowAsPopupAsync(mainWindow);
            }
            catch (Exception exception)
            {
                var exceptionBox = MessageBoxManager.GetMessageBoxStandard(exception.Message, "'Rename machine' was canceled", ButtonEnum.Ok, Icon.Error);
                await exceptionBox.ShowAsPopupAsync(mainWindow);
            }

            _reloadReactiveCommand.Run();
        }
    }
}