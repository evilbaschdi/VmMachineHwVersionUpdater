using Avalonia.Controls;
using Avalonia.Input;
using EvilBaschdi.About.Avalonia;
using EvilBaschdi.About.Avalonia.Models;
using EvilBaschdi.About.Core;
using EvilBaschdi.Core;
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
        IHandleOsDependentTitleBar handleOsDependentTitleBar = new HandleOsDependentTitleBar();
        handleOsDependentTitleBar.RunFor((this, HeaderPanel, MainPanel, AcrylicBorder));
    }

    // ReSharper disable UnusedParameter.Local
    private void LogoOnTapped(object sender, TappedEventArgs e)
        // ReSharper restore UnusedParameter.Local
    {
        ICurrentAssembly currentAssembly = new CurrentAssembly();
        IAboutContent aboutContent = new AboutContent(currentAssembly);
        IAboutViewModelExtended aboutViewModelExtended = new AboutViewModelExtended(aboutContent);
        var aboutWindow = new AboutWindow
                          {
                              DataContext = aboutViewModelExtended
                          };
        aboutWindow.ShowDialog(this);
    }
}