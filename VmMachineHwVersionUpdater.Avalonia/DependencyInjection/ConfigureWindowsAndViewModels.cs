using VmMachineHwVersionUpdater.Avalonia.ViewModels;
using VmMachineHwVersionUpdater.Avalonia.Views;

namespace VmMachineHwVersionUpdater.Avalonia.DependencyInjection;

/// <inheritdoc />
public class ConfigureWindowsAndViewModels : IConfigureWindowsAndViewModels
{
    /// <inheritdoc />
    public void RunFor([NotNull] IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<AddEditAnnotationDialogViewModel>();
        services.AddTransient(typeof(AddEditAnnotationDialog));

        services.AddSingleton<MainWindowViewModel>();
    }
}