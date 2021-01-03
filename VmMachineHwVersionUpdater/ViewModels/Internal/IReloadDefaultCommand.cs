using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc cref="ITaskRunDefaultCommand" />
    /// <inheritdoc cref="IDialogCoordinatorContext" />
    public interface IReloadDefaultCommand : ITaskRunDefaultCommand, IDialogCoordinatorContext
    {
    }
}