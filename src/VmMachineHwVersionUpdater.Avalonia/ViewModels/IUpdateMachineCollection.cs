namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <summary>
///     Handles adding, removing, and refreshing machines in the UI-bound collection on the UI thread.
/// </summary>
public interface IUpdateMachineCollection
{
    /// <summary>
    ///     Removes all entries matching <paramref name="filePath" /> and adds the new <paramref name="machine" />.
    /// </summary>
    void ReplaceByPath(LoadHelper loadValue, string filePath, Machine machine);

    /// <summary>
    ///     Removes all entries matching <paramref name="filePath" /> from the collection.
    /// </summary>
    void RemoveByPath(LoadHelper loadValue, string filePath);
}