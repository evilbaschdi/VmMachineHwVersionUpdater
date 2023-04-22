using Avalonia.Controls;
using EvilBaschdi.Core.Avalonia;

namespace VmMachineHwVersionUpdater.Avalonia.Views;

/// <inheritdoc />
public partial class MainWindow : Window
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        Load();
    }

    private void Load()
    {
        var handleOsDependentTitleBar = App.ServiceProvider?.GetRequiredService<IHandleOsDependentTitleBar>();
        handleOsDependentTitleBar?.RunFor((this, HeaderPanel, MainPanel, AcrylicBorder));

        var applicationLayout = App.ServiceProvider?.GetRequiredService<IApplicationLayout>();
        applicationLayout?.Run();
    }
}