using VmMachineHwVersionUpdater.Wpf.ViewModels;
using VmMachineHwVersionUpdater.Wpf.Views;

namespace VmMachineHwVersionUpdater.Wpf.DependencyInjection;

/// <summary />
public static class ConfigureWindowsAndViewModels
{
    /// <summary />
    public static void AddWindowsAndViewModels(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IAddEditAnnotationDialogViewModel, AddEditAnnotationDialogViewModel>();
        services.AddTransient<AddEditAnnotationDialog>();

        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient<MainWindow>();
    }
}