using EvilBaschdi.Core.Avalonia;
using EvilBaschdi.Core.Avalonia.Internal;

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
            await MessageBox.Show(mainWindow, "Machine is currently read only", "'Rename machine' was canceled", MessageBoxButtons.Ok, MessageBoxType.Warning);
            return;
        }

        var result = await MessageBox.Show(mainWindow, $"Are you sure you want to rename machine '{machine.DisplayName}'?", "Rename machine...", MessageBoxButtons.YesNo);
        if (result == MessageBoxResult.Yes)
        {
            try
            {
                var inputResult = await DialogBox.Show(mainWindow, "Enter the new display name", "Rename machine...");

                if (inputResult != null)
                {
                    _changeDisplayName.RunFor(machine.Path, inputResult);
                }
            }
            catch (IOException ioException)
            {
                await MessageBox.Show(mainWindow, ioException.Message, "'Rename machine' was canceled", MessageBoxButtons.Ok, MessageBoxType.Error);
            }
            catch (Exception exception)
            {
                await MessageBox.Show(mainWindow, exception.Message, "'Rename machine' was canceled", MessageBoxButtons.Ok, MessageBoxType.Error);
            }

            _reloadReactiveCommand.Run();
        }
    }
}