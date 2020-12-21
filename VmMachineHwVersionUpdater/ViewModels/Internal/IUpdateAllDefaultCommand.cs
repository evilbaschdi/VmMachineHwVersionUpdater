using EvilBaschdi.Core;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc cref="ITaskRunDefaultCommand" />
    /// <inheritdoc cref="IRun" />
    public interface IUpdateAllDefaultCommand : ITaskRunDefaultCommand, IRun
    {
    }
}