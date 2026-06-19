namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <summary>
///     Updates machine state and log information when .vmss or .log files change.
/// </summary>
public interface IUpdateMachineStateFromFile
{
    /// <summary>
    ///     Updates the machine state or log info for the machine located in the same directory as <paramref name="filePath" />
    ///     .
    /// </summary>
    void UpdateFor(string filePath, LoadHelper loadValue);
}