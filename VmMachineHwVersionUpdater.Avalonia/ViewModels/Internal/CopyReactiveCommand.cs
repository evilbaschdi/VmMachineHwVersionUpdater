using EvilBaschdi.Core.Avalonia;
using EvilBaschdi.Core.Avalonia.Internal;

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
            await MessageBox.Show(mainWindow, "Machine is currently read only", "'Copy machine' was canceled", MessageBoxButtons.Ok, MessageBoxType.Warning);
            return;
        }

        var result = await MessageBox.Show(mainWindow, $"Are you sure you want to copy machine '{machine.DisplayName}'?", "Copy machine...", MessageBoxButtons.YesNo);
        if (result == MessageBoxResult.Yes)
        {
            try
            {
                var inputResult = await DialogBox.Show(mainWindow, "Enter the new directory name", "Copy machine...");

                if (inputResult != null)
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

                                       await _copyMachine.ValueFor(machine, inputResult);
                                   });
                    //await controller.CloseAsync();
                }
            }
            catch (IOException ioException)
            {
                await MessageBox.Show(mainWindow, ioException.Message, "'Copy machine' was canceled", MessageBoxButtons.Ok, MessageBoxType.Error);
            }
            catch (Exception exception)
            {
                await MessageBox.Show(mainWindow, exception.Message, "'Copy machine' was canceled", MessageBoxButtons.Ok, MessageBoxType.Error);
            }

            _reloadReactiveCommand.Run();
        }
    }
}