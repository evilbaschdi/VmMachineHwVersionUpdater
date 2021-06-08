using System;
using System.ComponentModel;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using JetBrains.Annotations;
using VmMachineHwVersionUpdater.Core.PerMachine;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc />
    public class AddEditAnnotationDefaultCommand : IAddEditAnnotationDefaultCommand
    {
        private readonly IAddEditAnnotation _addEditAnnotation;
        private readonly IReloadDefaultCommand _reloadDefaultCommand;
        private readonly ISelectedMachine _selectedMachine;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="selectedMachine"></param>
        /// <param name="reloadDefaultCommand"></param>
        /// <param name="addEditAnnotation"></param>
        public AddEditAnnotationDefaultCommand([NotNull] ISelectedMachine selectedMachine, [NotNull] IReloadDefaultCommand reloadDefaultCommand,
                                               [NotNull] IAddEditAnnotation addEditAnnotation)
        {
            _selectedMachine = selectedMachine ?? throw new ArgumentNullException(nameof(selectedMachine));
            _reloadDefaultCommand = reloadDefaultCommand ?? throw new ArgumentNullException(nameof(reloadDefaultCommand));
            _addEditAnnotation = addEditAnnotation ?? throw new ArgumentNullException(nameof(addEditAnnotation));
        }

        /// <inheritdoc />
        public DefaultCommand Value => new()
                                       {
                                           Command = new RelayCommand(_ => Run())
                                       };

        /// <inheritdoc />
        public void Run()
        {
            var addEditAnnotationDialog = new AddEditAnnotationDialog(_addEditAnnotation, _selectedMachine.Value)
                                          {
                                              DataContext = new AddEditAnnotationDialogViewModel()
                                          };
            addEditAnnotationDialog.Closing += RunFor;
            addEditAnnotationDialog.ShowDialog();
        }

        /// <inheritdoc />
        public async void RunFor(object valueIn1, CancelEventArgs valueIn2)
        {
            if (valueIn1 is null)
            {
                throw new ArgumentNullException(nameof(valueIn1));
            }

            if (valueIn2 is null)
            {
                throw new ArgumentNullException(nameof(valueIn2));
            }
            
            await _reloadDefaultCommand.RunAsync();
        }
    }
}