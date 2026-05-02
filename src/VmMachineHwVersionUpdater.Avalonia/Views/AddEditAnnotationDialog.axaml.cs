using EvilBaschdi.Core.Avalonia.DependencyInjection;
using EvilBaschdi.Core.Avalonia.Themes;
using FluentAvalonia.UI.Windowing;
using VmMachineHwVersionUpdater.Avalonia.ViewModels;

namespace VmMachineHwVersionUpdater.Avalonia.Views;

/// <inheritdoc />
public partial class AddEditAnnotationDialog : FAAppWindow
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public AddEditAnnotationDialog()
    {
        InitializeComponent();
        ThemeEngine.ApplyThemeToWindow(this, false);
        DataContext = ApplicationServices.GetRequiredService<AddEditAnnotationDialogViewModel>();
    }
}