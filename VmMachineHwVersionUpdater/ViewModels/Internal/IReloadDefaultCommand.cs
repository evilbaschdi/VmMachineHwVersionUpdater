﻿using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;

namespace VmMachineHwVersionUpdater.ViewModels.Internal;

/// <inheritdoc cref="IDefaultCommandRunAsync" />
/// <inheritdoc cref="IDialogCoordinatorContext" />
public interface IReloadDefaultCommand : IDefaultCommandRunAsync, IDialogCoordinatorContext
{
}