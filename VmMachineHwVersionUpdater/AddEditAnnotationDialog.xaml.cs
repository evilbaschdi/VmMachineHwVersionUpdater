﻿using System;
using System.Windows;
using JetBrains.Annotations;
using MahApps.Metro.Controls;
using VmMachineHwVersionUpdater.Core.Models;
using VmMachineHwVersionUpdater.Core.PerMachine;

namespace VmMachineHwVersionUpdater
{
    /// <summary>
    ///     Interaction logic for AddEditAnnotationDialog.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class AddEditAnnotationDialog : MetroWindow
    {
        private readonly IAddEditAnnotation _addEditAnnotation;
        private readonly string _currentAnnotation;
        private Machine _machine;

        /// <inheritdoc />
        /// <summary>
        ///     Constructor
        /// </summary>
        public AddEditAnnotationDialog(IAddEditAnnotation addEditAnnotation, [NotNull] Machine machine)
        {
            _machine = machine ?? throw new ArgumentNullException(nameof(machine));
            _addEditAnnotation = addEditAnnotation ?? throw new ArgumentNullException(nameof(addEditAnnotation));
            InitializeComponent();
            _currentAnnotation = _machine.Annotation;
            Annotation.Text = _currentAnnotation;
        }

        private void UpdateAnnotationClick(object sender, RoutedEventArgs e)
        {
            var newAnnotation = Annotation.Text;

            if (!_currentAnnotation.Equals(newAnnotation) && _machine != null)
            {
                _addEditAnnotation.RunFor(_machine.Path, newAnnotation.Replace("\r", "|0D").Replace("\n", "|0A"));
            }

            _machine = null;
            _addEditAnnotation.Dispose();
        }
    }
}