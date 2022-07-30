using EvilBaschdi.CoreExtended.Controls.About;
using VmMachineHwVersionUpdater.ViewModels;

namespace VmMachineHwVersionUpdater.DependencyInjection;

/// <inheritdoc />
public class ConfigureWindowsAndViewModels : IConfigureWindowsAndViewModels
{
    /// <inheritdoc />
    public void RunFor([NotNull] IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddSingleton<AddEditAnnotationDialogViewModel>();
        services.AddTransient(typeof(AddEditAnnotationDialog));

        services.AddSingleton<IAboutModel, AboutViewModel>();
        services.AddTransient(typeof(AboutWindow));

        services.AddSingleton<MainWindowViewModel>();
        services.AddTransient(typeof(MainWindow));
    }
}