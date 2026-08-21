namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <inheritdoc cref="IConcurrentDictionaryOf{TKey, TValue}" />
public interface IGuestOsesInUse : IConcurrentDictionaryOf<string, bool>;