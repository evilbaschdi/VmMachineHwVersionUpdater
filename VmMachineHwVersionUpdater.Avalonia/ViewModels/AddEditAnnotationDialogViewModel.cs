namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <inheritdoc cref="IAddEditAnnotationDialogViewModel" />
public class AddEditAnnotationDialogViewModel(
    [NotNull] IUpdateAnnotation updateAnnotation,
    [NotNull] ICurrentMachine currentMachine) : ViewModelBase, IAddEditAnnotationDialogViewModel
{
    private readonly ICurrentMachine _currentMachine = currentMachine ?? throw new ArgumentNullException(nameof(currentMachine));
    private readonly IUpdateAnnotation _updateAnnotation = updateAnnotation ?? throw new ArgumentNullException(nameof(updateAnnotation));

    #region Properties

    /// <summary>
    ///     Binding
    /// </summary>
    public Machine SelectedMachine
    {
        get => _currentMachine.Value;
        set => _currentMachine.Value = value;
    }

    /// <inheritdoc />
    public string AnnotationText
    {
        get => _updateAnnotation.Value;
        set => _updateAnnotation.Value = value;
    }

    #endregion Properties
}