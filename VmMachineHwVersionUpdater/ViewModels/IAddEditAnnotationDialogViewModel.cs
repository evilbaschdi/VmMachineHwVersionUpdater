using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;

namespace VmMachineHwVersionUpdater.ViewModels
{
    /// <summary />
    public interface IAddEditAnnotationDialogViewModel
    {
        /// <summary />
        public ICommandViewModel UpdateAnnotationClick { get; set; }

        /// <summary />
        public string AnnotationText { get; set; }
    }
}