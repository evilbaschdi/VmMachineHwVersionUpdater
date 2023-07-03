using System.IO;
using MahApps.Metro.Controls.Dialogs;

namespace VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

/// <inheritdoc />
public class ArchiveDefaultCommand : IArchiveDefaultCommand
{
    [NotNull] private readonly IArchiveMachine _archiveMachine;
    [NotNull] private readonly ICurrentItem _currentItem;
    [NotNull] private readonly IDialogCoordinator _dialogCoordinator;
    [NotNull] private readonly IReloadDefaultCommand _reloadDefaultCommand;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="reloadDefaultCommand"></param>
    /// <param name="currentItem"></param>
    /// <param name="archiveMachine"></param>
    /// <param name="instance"></param>
    public ArchiveDefaultCommand([NotNull] IDialogCoordinator instance,
                                 [NotNull] IReloadDefaultCommand reloadDefaultCommand,
                                 [NotNull] ICurrentItem currentItem,
                                 [NotNull] IArchiveMachine archiveMachine)
    {
        _currentItem = currentItem ?? throw new ArgumentNullException(nameof(currentItem));
        _archiveMachine = archiveMachine ?? throw new ArgumentNullException(nameof(archiveMachine));
        _dialogCoordinator = instance ?? throw new ArgumentNullException(nameof(instance));
        _reloadDefaultCommand = reloadDefaultCommand ?? throw new ArgumentNullException(nameof(reloadDefaultCommand));
    }

    /// <inheritdoc />
    public DefaultCommand DefaultCommandValue
    {
        get
        {
            async void Execute(object _) => await Value();

            return new()
                   {
                       Command = new RelayCommand(Execute)
                   };
        }
    }

    /// <inheritdoc />
    public async Task Value()
    {
        var result = await _dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "Archive machine...",
            $"Are you sure you want to archive machine '{_currentItem.Value.DisplayName}'?",
            MessageDialogStyle.AffirmativeAndNegative).ConfigureAwait(true);

        if (result == MessageDialogResult.Affirmative)
        {
            try
            {
                _archiveMachine.RunFor(_currentItem.Value);

                await _reloadDefaultCommand.Value();
            }
            catch (IOException ioException)
            {
                await _dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "'Archive machine' was canceled", ioException.Message);
            }
            catch (Exception exception)
            {
                await _dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "'Archive machine' was canceled", exception.Message);
            }
        }
    }

    /// <inheritdoc />
    public object DialogCoordinatorContext { get; set; }
}