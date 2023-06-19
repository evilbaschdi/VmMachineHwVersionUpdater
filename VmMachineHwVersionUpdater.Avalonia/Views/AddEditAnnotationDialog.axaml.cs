using Avalonia.Controls;
using EvilBaschdi.Core.Avalonia;
using VmMachineHwVersionUpdater.Avalonia.ViewModels;

namespace VmMachineHwVersionUpdater.Avalonia.Views;

/// <inheritdoc />
public partial class AddEditAnnotationDialog : Window
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public AddEditAnnotationDialog()
    {
        InitializeComponent();
        ApplyLayout();

        DataContext = App.ServiceProvider.GetRequiredService<AddEditAnnotationDialogViewModel>();
    }

    private void ApplyLayout()
    {
        var handleOsDependentTitleBar = App.ServiceProvider?.GetRequiredService<IHandleOsDependentTitleBar>();
        handleOsDependentTitleBar?.RunFor(this);

        var applicationLayout = App.ServiceProvider?.GetRequiredService<IApplicationLayout>();
        applicationLayout?.RunFor((this, true, true));
    }
}