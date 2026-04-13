using Avalonia.Controls;
using EvilBaschdi.Core.Avalonia.Behaviors;
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
        Opened += OnOpened;
        DataContext = ApplicationServices.GetRequiredService<AddEditAnnotationDialogViewModel>();
    }

    private void ApplyLayout()
    {
        var handleOsDependentTitleBar = ApplicationServices.GetRequiredService<IHandleOsDependentTitleBar>();
        handleOsDependentTitleBar?.RunFor(this);

        var applicationLayout = ApplicationServices.GetRequiredService<IApplicationLayout>();
        applicationLayout?.RunFor((this, true, false));
    }

    private void OnOpened(object sender, EventArgs e)
    {
        if (sender is not Window window)
        {
            return;
        }

        var windowOpenedBehavior = ApplicationServices.ServiceProvider?.GetRequiredService<IWindowOpenedBehavior>();
        windowOpenedBehavior?.OnWindowOpened(window);
    }
}