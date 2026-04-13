using EvilBaschdi.Core.Avalonia.DependencyInjection;
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
        ApplyLayout();

        DataContext = ApplicationServices.GetRequiredService<AddEditAnnotationDialogViewModel>();
    }

    private void ApplyLayout()
    {
        var handleOsDependentTitleBar = ApplicationServices.GetRequiredService<IHandleOsDependentTitleBar>();
        handleOsDependentTitleBar?.RunFor(this);

        var applicationLayout = ApplicationServices.GetRequiredService<IApplicationLayout>();
        applicationLayout?.RunFor((this, true, false));
    }
}