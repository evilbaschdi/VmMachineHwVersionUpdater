using EvilBaschdi.About.Core.Models;
using EvilBaschdi.About.Wpf;
using VmMachineHwVersionUpdater.Core.DependencyInjection;
using VmMachineHwVersionUpdater.ViewModels;

namespace VmMachineHwVersionUpdater.DependencyInjection;

/// <inheritdoc />
public class ConfigureWindowsAndViewModels : IConfigureWindowsAndViewModels
{
    /// <inheritdoc />
    public void RunFor([NotNull] IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<AddEditAnnotationDialogViewModel>();
        services.AddTransient(typeof(AddEditAnnotationDialog));

        services.AddSingleton<IAboutViewModel, AboutViewModel>();
        services.AddTransient(typeof(AboutWindow));

        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient(typeof(MainWindow));
    }
}