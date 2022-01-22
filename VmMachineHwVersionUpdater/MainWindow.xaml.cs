using System;
using System.Windows;
using MahApps.Metro.Controls;
using Microsoft.Extensions.DependencyInjection;
using VmMachineHwVersionUpdater.ViewModels;

namespace VmMachineHwVersionUpdater;

/// <inheritdoc cref="MetroWindow" />
/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
// ReSharper disable once RedundantExtendsListEntry
public partial class MainWindow : MetroWindow
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
        Loaded += MainWindowLoaded;
    }

    private void MainWindowLoaded(object sender, RoutedEventArgs e)
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