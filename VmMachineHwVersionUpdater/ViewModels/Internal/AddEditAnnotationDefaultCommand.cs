using System;
using System.ComponentModel;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using VmMachineHwVersionUpdater.Core.PerMachine;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc />
    public class AddEditAnnotationDefaultCommand : IAddEditAnnotationDefaultCommand
    {
        private readonly IReloadDefaultCommand _reloadDefaultCommand;
        private readonly IServiceProvider _serviceProvider;
        private readonly IUpdateAnnotation _updateAnnotation;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="reloadDefaultCommand"></param>
        /// <param name="updateAnnotation"></param>
        /// <param name="serviceProvider"></param>
        public AddEditAnnotationDefaultCommand([NotNull] IReloadDefaultCommand reloadDefaultCommand, [NotNull] IUpdateAnnotation updateAnnotation,
                                               [NotNull] IServiceProvider serviceProvider)
        {
            _reloadDefaultCommand = reloadDefaultCommand ?? throw new ArgumentNullException(nameof(reloadDefaultCommand));
            _updateAnnotation = updateAnnotation ?? throw new ArgumentNullException(nameof(updateAnnotation));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        /// <inheritdoc />
        public DefaultCommand Value => new()
                                       {
                                           Command = new RelayCommand(_ => Run())
                                       };

        /// <inheritdoc />
        public void Run()
        {
            //var addEditAnnotationDialog = new AddEditAnnotationDialog
            //{
            //    //DataContext = new AddEditAnnotationDialogViewModel(_updateAnnotation)
            //};


            var addEditAnnotationDialog = _serviceProvider.GetRequiredService<AddEditAnnotationDialog>();
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

            // await _reloadDefaultCommand.RunAsync();
        }
    }
}