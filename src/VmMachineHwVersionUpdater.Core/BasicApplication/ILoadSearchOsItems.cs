using System.Collections.ObjectModel;

namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <summary>
///     Provides the cached operating-system filter items.
/// </summary>
public interface ILoadSearchOsItems : IWritableValue<ObservableCollection<object>>,
    ICachedValue<ObservableCollection<object>>;