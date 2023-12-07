namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
/// <summary>
///     Constructor
/// </summary>
/// <param name="currentItem"></param>
/// <param name="addEditAnnotation"></param>
public class UpdateAnnotation([NotNull] ICurrentItem currentItem, [NotNull] IAddEditAnnotation addEditAnnotation) : IUpdateAnnotation
{
    private readonly IAddEditAnnotation _addEditAnnotation = addEditAnnotation ?? throw new ArgumentNullException(nameof(addEditAnnotation));
    private readonly ICurrentItem _currentItem = currentItem ?? throw new ArgumentNullException(nameof(currentItem));

    /// <inheritdoc cref="string" />
    public string Value
    {
        get => _currentItem.Value?.Annotation;
        set
        {
            var newAnnotation = value ?? string.Empty;
            var machine = _currentItem.Value;

            if (machine is { IsEnabledForEditing: true } && !Value.Equals(newAnnotation))
            {
                _addEditAnnotation.RunFor(machine.Path, newAnnotation.Replace("\r", "|0D").Replace("\n", "|0A"));
            }

            _currentItem.Value.Annotation = newAnnotation;
            //_currentItem.Value = null;
            //_addEditAnnotation.Dispose();
        }
    }
}