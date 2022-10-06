using EvilBaschdi.CoreExtended;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel;
using VmMachineHwVersionUpdater.Core.Models;
using VmMachineHwVersionUpdater.Core.PerMachine;

namespace VmMachineHwVersionUpdater.ViewModels;

/// <inheritdoc cref="IAddEditAnnotationDialogViewModel" />
public class AddEditAnnotationDialogViewModel : ApplicationStyleViewModel, IAddEditAnnotationDialogViewModel
{
    private readonly ICurrentItem _currentItem;
    private readonly IUpdateAnnotation _updateAnnotation;

    #region Constructor

    /// <summary>
    ///     Constructor
    /// </summary>
    public AddEditAnnotationDialogViewModel(IApplicationStyle applicationStyle, [NotNull] IUpdateAnnotation updateAnnotation, [NotNull] ICurrentItem currentItem)
        : base(applicationStyle)
    {
        _updateAnnotation = updateAnnotation ?? throw new ArgumentNullException(nameof(updateAnnotation));
        _currentItem = currentItem ?? throw new ArgumentNullException(nameof(currentItem));
    }

    #endregion Constructor

    #region Properties

    /// <summary>
    ///     Binding
    /// </summary>
    public Machine SelectedMachine
    {
        get => _currentItem.Value;
        set
        {
            _currentItem.Value = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc />
    public string AnnotationText
    {
        get => _updateAnnotation.Value;
        set => _updateAnnotation.Value = value;
    }

    #endregion Properties
}