using System.IO;
using MahApps.Metro.Controls.Dialogs;

namespace VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

/// <inheritdoc />
public class ArchiveDefaultCommand(
    [NotNull] IDialogCoordinator instance,
    [NotNull] IReloadDefaultCommand reloadDefaultCommand,
    [NotNull] ICurrentMachine currentMachine,
    [NotNull] IArchiveMachine archiveMachine) : IArchiveDefaultCommand
{
    [NotNull] private readonly IArchiveMachine _archiveMachine = archiveMachine ?? throw new ArgumentNullException(nameof(archiveMachine));
    [NotNull] private readonly ICurrentMachine _currentMachine = currentMachine ?? throw new ArgumentNullException(nameof(currentMachine));
    [NotNull] private readonly IDialogCoordinator _dialogCoordinator = instance ?? throw new ArgumentNullException(nameof(instance));
    [NotNull] private readonly IReloadDefaultCommand _reloadDefaultCommand = reloadDefaultCommand ?? throw new ArgumentNullException(nameof(reloadDefaultCommand));

    /// <inheritdoc />
    public DefaultCommand DefaultCommandValue
    {
        get
        {
            return new()
                   {
                       Command = new RelayCommand(Execute)
                   };

            
            async void Execute(object _) => await RunAsync();
        }
    }

    /// <inheritdoc />
    public async Task RunAsync()
    {
        var result = await _dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "Archive machine...",
            $"Are you sure you want to archive machine '{_currentMachine.Value.DisplayName}'?",
            MessageDialogStyle.AffirmativeAndNegative).ConfigureAwait(true);

        if (result == MessageDialogResult.Affirmative)
        {
            try
            {
                _archiveMachine.RunFor(_currentMachine.Value);

                await _reloadDefaultCommand.RunAsync();
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