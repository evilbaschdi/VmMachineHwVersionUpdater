using System.Windows;
using MahApps.Metro.Controls;
using Microsoft.Extensions.DependencyInjection;
using VmMachineHwVersionUpdater.ViewModels;

namespace VmMachineHwVersionUpdater;

/// <inheritdoc cref="MetroWindow" />
/// <summary>
///     Interaction logic for MainOnLoaded.xaml
/// </summary>
// ReSharper disable once RedundantExtendsListEntry
public partial class MainOnLoaded : MetroWindow, IOnLoaded
{
    private readonly IServiceProvider _serviceProvider;

    /// <inheritdoc />
    /// <summary>
    ///     Constructor
    /// </summary>
    public MainOnLoaded()
    {
        InitializeComponent();

        _serviceProvider = App.ServiceProvider;
        Loaded += RunFor;
    }

    /// <inheritdoc />
    public void RunFor(object sender, RoutedEventArgs e)
    {
        if (sender == null)
        {
            throw new ArgumentNullException(nameof(sender));
        }

        if (e == null)
        {
            throw new ArgumentNullException(nameof(e));
        }

        DataContext = ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, typeof(MainWindowViewModel));
    }
}