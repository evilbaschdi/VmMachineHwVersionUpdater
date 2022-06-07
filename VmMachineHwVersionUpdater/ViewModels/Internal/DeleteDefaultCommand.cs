using System.IO;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using JetBrains.Annotations;
using MahApps.Metro.Controls.Dialogs;
using VmMachineHwVersionUpdater.Core.Models;
using VmMachineHwVersionUpdater.Core.PerMachine;

namespace VmMachineHwVersionUpdater.ViewModels.Internal;

/// <inheritdoc />
public class DeleteDefaultCommand : IDeleteDefaultCommand
{
    private readonly ICurrentItem _currentItem;
    private readonly IDeleteMachine _deleteMachine;
    private readonly IDialogCoordinator _dialogCoordinator;
    private readonly IReloadDefaultCommand _reloadDefaultCommand;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="dialogCoordinator"></param>
    /// <param name="currentItem"></param>
    /// <param name="deleteMachine"></param>
    /// <param name="reloadDefaultCommand"></param>
    public DeleteDefaultCommand([NotNull] IDialogCoordinator dialogCoordinator, [NotNull] ICurrentItem currentItem, [NotNull] IDeleteMachine deleteMachine,
                                [NotNull] IReloadDefaultCommand reloadDefaultCommand)
    {
        _dialogCoordinator = dialogCoordinator ?? throw new ArgumentNullException(nameof(dialogCoordinator));
        _currentItem = currentItem ?? throw new ArgumentNullException(nameof(currentItem));
        _deleteMachine = deleteMachine ?? throw new ArgumentNullException(nameof(deleteMachine));
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
        var result = await _dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "Delete machine...",
            $"Are you sure you want to delete '{_currentItem.Value.DisplayName}'?",
            MessageDialogStyle.AffirmativeAndNegative).ConfigureAwait(true);

        if (result == MessageDialogResult.Affirmative)
        {
            try
            {
                _deleteMachine.RunFor(_currentItem.Value.Path);

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