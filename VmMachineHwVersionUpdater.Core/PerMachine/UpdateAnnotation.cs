namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
/// <summary>
///     Constructor
/// </summary>
/// <param name="currentMachine"></param>
/// <param name="addEditAnnotation"></param>
public class UpdateAnnotation(
    [NotNull] ICurrentMachine currentMachine,
    [NotNull] IAddEditAnnotation addEditAnnotation) : IUpdateAnnotation
{
    private readonly IAddEditAnnotation _addEditAnnotation = addEditAnnotation ?? throw new ArgumentNullException(nameof(addEditAnnotation));
    private readonly ICurrentMachine _currentMachine = currentMachine ?? throw new ArgumentNullException(nameof(currentMachine));

    /// <inheritdoc cref="string" />
    public string Value
    {
        get => _currentMachine.Value?.Annotation;
        set
        {
            var newAnnotation = value ?? string.Empty;
            var machine = _currentMachine.Value;

            if (machine is { IsEnabledForEditing: true } && !Value.Equals(newAnnotation))
            {
                _addEditAnnotation.RunFor(machine.Path, newAnnotation.Replace("\r", "|0D").Replace("\n", "|0A"));
            }

            _currentMachine.Value.Annotation = newAnnotation;
            //_currentMachine.Value = null;
            //_addEditAnnotation.Dispose();
        }
    }
}