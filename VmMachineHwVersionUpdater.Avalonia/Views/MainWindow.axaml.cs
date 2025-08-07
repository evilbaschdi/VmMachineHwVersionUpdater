using Avalonia;
using Avalonia.Controls;

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

#if DEBUG

        this.AttachDevTools();
#endif
    }
}