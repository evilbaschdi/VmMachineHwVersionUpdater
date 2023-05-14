using EvilBaschdi.About.Avalonia;
using EvilBaschdi.About.Avalonia.Models;
using EvilBaschdi.Core.Avalonia;
using VmMachineHwVersionUpdater.Avalonia.ViewModels;

namespace VmMachineHwVersionUpdater.Avalonia.DependencyInjection;

/// <inheritdoc />
public class ConfigureWindowsAndViewModels : IConfigureWindowsAndViewModels
{
    /// <inheritdoc />
    public void RunFor([NotNull] IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        //services.AddSingleton<AddEditAnnotationDialogViewModel>();
        //services.AddTransient(typeof(AddEditAnnotationDialog));

        services.AddSingleton<IAboutViewModelExtended, AboutViewModelExtended>();
        services.AddTransient(typeof(AboutWindow));

        services.AddSingleton<IHandleOsDependentTitleBar, HandleOsDependentTitleBar>();
        services.AddSingleton<IApplicationLayout, ApplicationLayout>();
        services.AddSingleton<MainWindowViewModel>();
    }
}