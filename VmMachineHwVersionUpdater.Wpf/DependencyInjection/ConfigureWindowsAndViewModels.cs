using VmMachineHwVersionUpdater.Wpf.ViewModels;
using VmMachineHwVersionUpdater.Wpf.Views;

namespace VmMachineHwVersionUpdater.Wpf.DependencyInjection;

/// <inheritdoc />
public class ConfigureWindowsAndViewModels : IConfigureWindowsAndViewModels
{
    /// <inheritdoc />
    public void RunFor([NotNull] IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IAddEditAnnotationDialogViewModel, AddEditAnnotationDialogViewModel>();
        services.AddTransient(typeof(AddEditAnnotationDialog));

        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient(typeof(MainWindow));
    }
}