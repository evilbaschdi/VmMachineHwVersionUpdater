using System;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using JetBrains.Annotations;
using VmMachineHwVersionUpdater.Core.PerMachine;

namespace VmMachineHwVersionUpdater.ViewModels
{
    /// <inheritdoc cref="IAddEditAnnotationDialogViewModel" />
    public class AddEditAnnotationDialogViewModel : ApplicationStyleViewModel, IAddEditAnnotationDialogViewModel
    {
        private readonly IUpdateAnnotation _updateAnnotation;

        #region Constructor

        /// <summary>
        ///     Constructor
        /// </summary>
        public AddEditAnnotationDialogViewModel([NotNull] IUpdateAnnotation updateAnnotation)
            : base(true)
        {
            _updateAnnotation = updateAnnotation ?? throw new ArgumentNullException(nameof(updateAnnotation));
        }

        #endregion Constructor

        #region Commands

        /// <inheritdoc />
        public ICommandViewModel UpdateAnnotationClick { get; set; }

        #endregion Commands

        #region Properties

        /// <inheritdoc />
        public string AnnotationText
        {
            get => _updateAnnotation.Value;
            set => _updateAnnotation.Value = value;
        }

        #endregion Properties
    }
}