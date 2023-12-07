namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <inheritdoc cref="IAddEditAnnotationDialogViewModel" />
/// <summary>
///     Constructor
/// </summary>
public class AddEditAnnotationDialogViewModel(
    [NotNull] IUpdateAnnotation updateAnnotation,
    [NotNull] ICurrentItem currentItem) : ViewModelBase, IAddEditAnnotationDialogViewModel
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