using EvilBaschdi.Core.Wpf;
using EvilBaschdi.Core.Wpf.Mvvm.ViewModel;

namespace VmMachineHwVersionUpdater.Wpf.ViewModels;

/// <inheritdoc cref="IAddEditAnnotationDialogViewModel" />
/// <summary>
///     Constructor
/// </summary>
public class AddEditAnnotationDialogViewModel(
    [NotNull] IApplicationLayout applicationLayout,
    [NotNull] IApplicationStyle applicationStyle,
    [NotNull] IUpdateAnnotation updateAnnotation,
    [NotNull] ICurrentMachine currentMachine) : ApplicationLayoutViewModel(applicationLayout, applicationStyle, true, false), IAddEditAnnotationDialogViewModel
{
    private readonly ICurrentMachine _currentMachine = currentMachine ?? throw new ArgumentNullException(nameof(currentMachine));
    private readonly IUpdateAnnotation _updateAnnotation = updateAnnotation ?? throw new ArgumentNullException(nameof(updateAnnotation));

    /// <summary>
    ///     Binding
    /// </summary>
    public Machine SelectedMachine
    {
        get => _currentMachine.Value;
        set
        {
            _currentMachine.Value = value;
            OnPropertyChanged();
        }
    }

    /// <inheritdoc />
    public string AnnotationText
    {
        get => _updateAnnotation.Value;
        set => _updateAnnotation.Value = value;
    }
}