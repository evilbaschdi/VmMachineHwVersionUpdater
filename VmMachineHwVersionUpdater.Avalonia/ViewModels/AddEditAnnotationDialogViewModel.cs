namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <inheritdoc cref="IAddEditAnnotationDialogViewModel" />
public class AddEditAnnotationDialogViewModel : ViewModelBase, IAddEditAnnotationDialogViewModel
{
    private readonly ICurrentItem _currentItem;
    private readonly IUpdateAnnotation _updateAnnotation;

    #region Constructor

    /// <summary>
    ///     Constructor
    /// </summary>
    public AddEditAnnotationDialogViewModel(
        [NotNull] IUpdateAnnotation updateAnnotation,
        [NotNull] ICurrentItem currentItem)

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
        set => _currentItem.Value = value;
    }

    /// <inheritdoc />
    public string AnnotationText
    {
        get => _updateAnnotation.Value;
        set => _updateAnnotation.Value = value;
    }

    #endregion Properties
}