using System;
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
    public partial class AddEditAnnotationDialog : MetroWindow
    {
        private readonly IAddEditAnnotation _addEditAnnotation;
        private readonly string _currentAnnotation;
        private Machine _machine;

        /// <inheritdoc />
        /// <summary>
        ///     Constructor
        /// </summary>
        public AddEditAnnotationDialog([NotNull] Machine machine)
        {
            _machine = machine ?? throw new ArgumentNullException(nameof(machine));
            InitializeComponent();
            _addEditAnnotation = new AddEditAnnotation();
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
        }
    }
}