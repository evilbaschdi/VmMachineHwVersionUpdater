using EvilBaschdi.Core.Avalonia.Behaviors;
using EvilBaschdi.Core.Avalonia.DependencyInjection;
using FluentAvalonia.UI.Windowing;

namespace VmMachineHwVersionUpdater.Avalonia.Views;

/// <inheritdoc />
public partial class MainWindow : FAAppWindow
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        Opened += OnOpened;

#if DEBUG

        //this.AttachDeveloperTools();

#endif
    }

    private void OnOpened(object sender, EventArgs e)
    {
        var windowOpenedBehavior = ApplicationServices.ServiceProvider?.GetRequiredService<IWindowOpenedBehavior>();
        windowOpenedBehavior?.OnWindowOpened(this);
    }
}