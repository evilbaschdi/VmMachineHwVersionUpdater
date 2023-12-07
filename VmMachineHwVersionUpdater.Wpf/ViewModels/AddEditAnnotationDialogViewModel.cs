using EvilBaschdi.Core.Wpf;
using EvilBaschdi.Core.Wpf.Mvvm.ViewModel;

namespace VmMachineHwVersionUpdater.Wpf.ViewModels;

/// <inheritdoc cref="IAddEditAnnotationDialogViewModel" />
/// <summary>
///     Constructor
/// </summary>
public class AddEditAnnotationDialogViewModel([NotNull] IApplicationLayout applicationLayout,
                                        [NotNull] IApplicationStyle applicationStyle,
                                        [NotNull] IUpdateAnnotation updateAnnotation,
                                        [NotNull] ICurrentItem currentItem) : ApplicationLayoutViewModel(applicationLayout, applicationStyle, true, false), IAddEditAnnotationDialogViewModel
{
    private readonly ICurrentItem _currentItem = currentItem ?? throw new ArgumentNullException(nameof(currentItem));
    private readonly IUpdateAnnotation _updateAnnotation = updateAnnotation ?? throw new ArgumentNullException(nameof(updateAnnotation));

    #region Constructor

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