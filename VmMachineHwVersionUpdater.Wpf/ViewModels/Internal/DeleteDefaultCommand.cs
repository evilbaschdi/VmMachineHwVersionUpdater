using System.IO;
using MahApps.Metro.Controls.Dialogs;

namespace VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

/// <inheritdoc />
/// <summary>
///     Constructor
/// </summary>
/// <param name="dialogCoordinator"></param>
/// <param name="currentMachine"></param>
/// <param name="deleteMachine"></param>
/// <param name="reloadDefaultCommand"></param>
public class DeleteDefaultCommand(
    [NotNull] IDialogCoordinator dialogCoordinator,
    [NotNull] ICurrentMachine currentMachine,
    [NotNull] IDeleteMachine deleteMachine,
    [NotNull] IReloadDefaultCommand reloadDefaultCommand) : IDeleteDefaultCommand
{
    private readonly ICurrentMachine _currentMachine = currentMachine ?? throw new ArgumentNullException(nameof(currentMachine));
    private readonly IDeleteMachine _deleteMachine = deleteMachine ?? throw new ArgumentNullException(nameof(deleteMachine));
    private readonly IDialogCoordinator _dialogCoordinator = dialogCoordinator ?? throw new ArgumentNullException(nameof(dialogCoordinator));
    private readonly IReloadDefaultCommand _reloadDefaultCommand = reloadDefaultCommand ?? throw new ArgumentNullException(nameof(reloadDefaultCommand));

    /// <inheritdoc />
    public DefaultCommand DefaultCommandValue
    {
        get
        {
            return new()
                   {
                       Command = new RelayCommand(Execute)
                   };

            async void Execute(object _) => await Value();
        }
    }

    /// <inheritdoc />
    public async Task Value()
    {
        var result = await _dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "Delete machine...",
            $"Are you sure you want to delete '{_currentMachine.Value.DisplayName}'?",
            MessageDialogStyle.AffirmativeAndNegative).ConfigureAwait(true);

        if (result == MessageDialogResult.Affirmative)
        {
            try
            {
                _deleteMachine.RunFor(_currentMachine.Value.Path);

                await _reloadDefaultCommand.Value();
            }
            catch (IOException ioException)
            {
                await _dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "'Delete machine' was canceled", ioException.Message);
            }
        }
    }

    /// <inheritdoc />
    public object DialogCoordinatorContext { get; set; }
}