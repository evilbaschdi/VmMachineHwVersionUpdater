using System.Windows;
using MahApps.Metro.Controls;
using VmMachineHwVersionUpdater.Wpf.Internal.Core;
using VmMachineHwVersionUpdater.Wpf.ViewModels;

namespace VmMachineHwVersionUpdater.Wpf.Views;

/// <inheritdoc cref="MetroWindow" />
/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
// ReSharper disable once RedundantExtendsListEntry
public partial class MainWindow : IOnLoaded
{
    private readonly IServiceProvider _serviceProvider;

    /// <inheritdoc />
    /// <summary>
    ///     Constructor
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();

        _serviceProvider = App.ServiceProvider;
        Loaded += RunFor;
    }

    /// <inheritdoc />
    public void RunFor(object sender, RoutedEventArgs e)
    {
        ArgumentNullException.ThrowIfNull(sender);
        ArgumentNullException.ThrowIfNull(e);

        DataContext = ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, typeof(MainWindowViewModel));
    }
}