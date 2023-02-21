using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.Core.PerMachine;

/// <inheritdoc />
public class UpdateAnnotation : IUpdateAnnotation
{
    private readonly IAddEditAnnotation _addEditAnnotation;
    private readonly ICurrentItem _currentItem;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="currentItem"></param>
    /// <param name="addEditAnnotation"></param>
    public UpdateAnnotation([NotNull] ICurrentItem currentItem, [NotNull] IAddEditAnnotation addEditAnnotation)
    {
        _currentItem = currentItem ?? throw new ArgumentNullException(nameof(currentItem));
        _addEditAnnotation = addEditAnnotation ?? throw new ArgumentNullException(nameof(addEditAnnotation));
    }

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

            //_currentItem.Value = null;
            _addEditAnnotation.Dispose();
        }
    }
}