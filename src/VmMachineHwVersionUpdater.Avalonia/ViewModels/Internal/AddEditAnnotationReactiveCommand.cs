using EvilBaschdi.Core.Avalonia.DependencyInjection;
using VmMachineHwVersionUpdater.Avalonia.Views;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IAddEditAnnotationReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandRxVoidTask" />
public class AddEditAnnotationReactiveCommand(
    [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime) : ReactiveCommandRxVoidTask, IAddEditAnnotationReactiveCommand
{
    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime =
        mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));

    /// <inheritdoc />
    public override async Task RunAsync(CancellationToken cancellationToken = default)
    {
        var addEditAnnotationDialog = ApplicationServices.GetRequiredService<AddEditAnnotationDialog>();
        var mainWindow = _mainWindowByApplicationLifetime.Value;
        if (mainWindow is not null)
        {
            await addEditAnnotationDialog.ShowDialog(mainWindow);
        }
    }
}