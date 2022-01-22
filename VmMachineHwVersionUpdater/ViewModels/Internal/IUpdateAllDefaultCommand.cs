using EvilBaschdi.Core;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;

namespace VmMachineHwVersionUpdater.ViewModels.Internal;

/// <inheritdoc cref="IDefaultCommandRunAsync" />
/// <inheritdoc cref="IRun" />
public interface IUpdateAllDefaultCommand : IDefaultCommandRunAsync, IRun
{
}