namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <inheritdoc cref="IWritableValue{T}" />
/// <inheritdoc cref="ICachedValue{T}" />
public interface ILoad : IWritableValue<LoadHelper>,
                         ICachedValue<LoadHelper>
{
    /// <summary>
    ///     Loads a fresh snapshot without replacing the current cached value.
    /// </summary>
    LoadHelper Refresh();
}