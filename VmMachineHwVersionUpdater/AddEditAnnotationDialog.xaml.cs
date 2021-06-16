using System;
using System.Windows;
using MahApps.Metro.Controls;
using Microsoft.Extensions.DependencyInjection;
using VmMachineHwVersionUpdater.ViewModels;

namespace VmMachineHwVersionUpdater
{
    /// <summary>
    ///     Interaction logic for AddEditAnnotationDialog.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class AddEditAnnotationDialog : MetroWindow
    {
        private readonly IServiceProvider _serviceProvider;

        /// <inheritdoc />
        /// <summary>
        ///     Constructor
        /// </summary>
        public AddEditAnnotationDialog()
        {
            _serviceProvider = App.ServiceProvider;
            Loaded += AddEditAnnotationDialogLoaded;
        }

        private void AddEditAnnotationDialogLoaded(object sender, RoutedEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            DataContext = ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, typeof(AddEditAnnotationDialogViewModel));
        }
    }
}