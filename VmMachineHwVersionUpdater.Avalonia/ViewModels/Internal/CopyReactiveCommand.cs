using Avalonia.Controls;
using EvilBaschdi.Core.Avalonia;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="ICopyReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
public class CopyReactiveCommand : ReactiveCommandUnitRun, ICopyReactiveCommand
{
    private readonly ICopyMachine _copyMachine;

    // private readonly ICopyProgress _copyProgress;
    private readonly ICurrentItem _currentItem;
    private readonly IReloadReactiveCommand _reloadReactiveCommand;
    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="currentItem"></param>
    /// <param name="reloadReactiveCommand"></param>
    /// <param name="mainWindowByApplicationLifetime"></param>
    /// <param name="copyMachine"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public CopyReactiveCommand(
        [NotNull] ICopyMachine copyMachine,
        //[NotNull] ICopyProgress copyProgress,
        [NotNull] ICurrentItem currentItem,
        [NotNull] IReloadReactiveCommand reloadReactiveCommand,
        [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime)
    {
        _copyMachine = copyMachine ?? throw new ArgumentNullException(nameof(copyMachine));
        //_copyProgress = copyProgress ?? throw new ArgumentNullException(nameof(copyProgress));
        _currentItem = currentItem ?? throw new ArgumentNullException(nameof(currentItem));
        _reloadReactiveCommand = reloadReactiveCommand ?? throw new ArgumentNullException(nameof(reloadReactiveCommand));
        _mainWindowByApplicationLifetime = mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));
    }

    /// <inheritdoc />
    public override async void Run()
    {
        var mainWindow = _mainWindowByApplicationLifetime.Value;
        var machine = _currentItem.Value;
        if (!machine.IsEnabledForEditing)
        {
            var warningBox = MessageBoxManager.GetMessageBoxStandard("Copy 'Copy machine' was canceled", "Machine is currently read only", ButtonEnum.Ok, Icon.Warning);
            await warningBox.ShowAsPopupAsync(mainWindow);
            return;
        }

        var box = MessageBoxManager.GetMessageBoxStandard("Copy machine...", $"Are you sure you want to copy machine '{machine.DisplayName}'?", ButtonEnum.YesNo, Icon.Question);
        var result = await box.ShowAsPopupAsync(mainWindow);

        if (result == ButtonResult.Yes)
        {
            try
            {
                var inputBox = MessageBoxManager.GetMessageBoxStandard(
                    new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.OkCancel,
                        ContentTitle = "Copy machine...",
                        //ContentHeader = "Copy machine...",
                        ContentMessage = "Enter the new directory name",
                        Icon = Icon.Folder,
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
                    await Task.Run(async () =>
                                   {
                                       //_copyProgress.Progress = new Progress<double>(increment =>
                                       //                                              {
                                       //                                                  controller.SetProgress(increment);
                                       //                                                  if (increment.Equals(100d))
                                       //                                                  {
                                       //                                                      controller.SetMessage("Finished process");
                                       //                                                  }
                                       //                                              });

                                       await _copyMachine.ValueFor(machine, inputBox.InputValue);
                                   });
                    //await controller.CloseAsync();
                }
            }
            catch (IOException ioException)
            {
                var ioExceptionBox = MessageBoxManager.GetMessageBoxStandard(ioException.Message, "'Copy machine' was canceled", ButtonEnum.Ok, Icon.Error);
                await ioExceptionBox.ShowAsPopupAsync(mainWindow);
            }
            catch (Exception exception)
            {
                var exceptionBox = MessageBoxManager.GetMessageBoxStandard(exception.Message, "'Copy machine' was canceled", ButtonEnum.Ok, Icon.Error);
                await exceptionBox.ShowAsPopupAsync(mainWindow);
            }

            _reloadReactiveCommand.Run();
        }
    }
}