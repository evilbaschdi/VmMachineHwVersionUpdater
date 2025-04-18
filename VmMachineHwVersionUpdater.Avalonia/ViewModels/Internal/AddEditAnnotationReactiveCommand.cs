﻿using EvilBaschdi.Core.Avalonia;
using VmMachineHwVersionUpdater.Avalonia.Views;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IAddEditAnnotationReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
public class AddEditAnnotationReactiveCommand(
    [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime) : ReactiveCommandUnitRun, IAddEditAnnotationReactiveCommand
{
    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime =
        mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));

    /// <inheritdoc />
    public override void Run()
    {
        var addEditAnnotationDialog = App.ServiceProvider.GetRequiredService<AddEditAnnotationDialog>();
        var mainWindow = _mainWindowByApplicationLifetime.Value;
        if (mainWindow != null)
        {
            addEditAnnotationDialog.ShowDialog(mainWindow);
        }
    }
}