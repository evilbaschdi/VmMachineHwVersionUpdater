using EvilBaschdi.Core.Avalonia;
using EvilBaschdi.Core.Avalonia.Internal;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IArchiveReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
public class ArchiveReactiveCommand : ReactiveCommandUnitRun, IArchiveReactiveCommand
{
    [NotNull] private readonly IArchiveMachine _archiveMachine;
    [NotNull] private readonly ICurrentItem _currentItem;
    [NotNull] private readonly IReloadReactiveCommand _reloadReactiveCommand;
    [NotNull] private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="archiveMachine"></param>
    /// <param name="currentItem"></param>
    /// <param name="reloadReactiveCommand"></param>
    /// <param name="mainWindowByApplicationLifetime"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public ArchiveReactiveCommand([NotNull] IArchiveMachine archiveMachine,
                                  [NotNull] ICurrentItem currentItem,
                                  [NotNull] IReloadReactiveCommand reloadReactiveCommand,
                                  [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime)
    {
        _archiveMachine = archiveMachine ?? throw new ArgumentNullException(nameof(archiveMachine));
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
            var title = "Archive machine...";
            var text = $"Are you sure you want to archive '{_currentItem.Value.DisplayName}'?";
            var result = await MessageBox.Show(mainWindow, text, title, MessageBoxButtons.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _archiveMachine.RunFor(_currentItem.Value);

                    _reloadReactiveCommand.Run();
                }
                catch (IOException ioException)
                {
                    await MessageBox.Show(mainWindow, ioException.Message, "'Archive machine' was canceled", MessageBoxButtons.Ok, MessageBoxType.Error);
                }

                catch (Exception exception)
                {
                    await MessageBox.Show(mainWindow, exception.Message, "'Archive machine' was canceled", MessageBoxButtons.Ok, MessageBoxType.Error);
                }
            }
        }
    }
}