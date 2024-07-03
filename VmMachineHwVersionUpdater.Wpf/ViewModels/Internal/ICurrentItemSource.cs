namespace VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

/// <inheritdoc cref="IWritableValue{T}" />
/// <inheritdoc cref="ICachedValue{T}" />
public interface ICurrentItemSource : IWritableValue<List<Machine>>, ICachedValue<List<Machine>>;