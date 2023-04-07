﻿using System.Windows;
using MahApps.Metro.Controls;
using VmMachineHwVersionUpdater.ViewModels;

namespace VmMachineHwVersionUpdater;

/// <summary>
///     Interaction logic for AddEditAnnotationDialog.xaml
/// </summary>
// ReSharper disable once RedundantExtendsListEntry
public partial class AddEditAnnotationDialog : MetroWindow, IOnLoaded
{
    private readonly IServiceProvider _serviceProvider;

    /// <inheritdoc />
    /// <summary>
    ///     Constructor
    /// </summary>
    public AddEditAnnotationDialog()
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

        DataContext = ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, typeof(AddEditAnnotationDialogViewModel));
    }
}