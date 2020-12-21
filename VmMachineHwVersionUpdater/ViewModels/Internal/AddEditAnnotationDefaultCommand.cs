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
        private readonly IInit _init;
        private readonly ISelectedMachine _selectedMachine;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="selectedMachine"></param>
        /// <param name="init"></param>
        public AddEditAnnotationDefaultCommand([NotNull] ISelectedMachine selectedMachine, [NotNull] IInit init)
        {
            _selectedMachine = selectedMachine ?? throw new ArgumentNullException(nameof(selectedMachine));
            _init = init ?? throw new ArgumentNullException(nameof(init));
        }

        /// <inheritdoc />
        public DefaultCommand Value => new()
                                       {
                                           Command = new RelayCommand(_ => Run())
                                       };

        /// <inheritdoc />
        public void Run()
        {
            IAddEditAnnotation addEditAnnotation = new AddEditAnnotation();
            var addEditAnnotationDialog = new AddEditAnnotationDialog(addEditAnnotation, _selectedMachine.Value)
                                          {
                                              DataContext = new AddEditAnnotationDialogViewModel()
                                          };
            addEditAnnotationDialog.Closing += RunFor;
            addEditAnnotationDialog.ShowDialog();
        }

        /// <inheritdoc />
        public void RunFor(object valueIn1, CancelEventArgs valueIn2)
        {
            if (valueIn1 is null)
            {
                throw new ArgumentNullException(nameof(valueIn1));
            }

            if (valueIn2 is null)
            {
                throw new ArgumentNullException(nameof(valueIn2));
            }

            _init.Run();
        }
    }
}