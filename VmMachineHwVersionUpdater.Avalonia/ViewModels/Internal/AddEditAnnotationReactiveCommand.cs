using EvilBaschdi.Core.Avalonia;
using VmMachineHwVersionUpdater.Avalonia.Views;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IAddEditAnnotationReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitTask" />
public class AddEditAnnotationReactiveCommand(
    [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime) : ReactiveCommandUnitTask, IAddEditAnnotationReactiveCommand
{
    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime =
        mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));

    /// <inheritdoc />
    public override async Task RunAsync()
    {
        var addEditAnnotationDialog = App.ServiceProvider.GetRequiredService<AddEditAnnotationDialog>();
        var mainWindow = _mainWindowByApplicationLifetime.Value;
        if (mainWindow != null)
        {
            await addEditAnnotationDialog.ShowDialog(mainWindow);
        }
    }
}