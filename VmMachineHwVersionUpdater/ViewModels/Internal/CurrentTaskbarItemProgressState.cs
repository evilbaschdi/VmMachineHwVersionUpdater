using System.Windows.Shell;

namespace VmMachineHwVersionUpdater.ViewModels.Internal;

/// <inheritdoc />
public class CurrentTaskbarItemProgressState : ITaskbarItemProgressState
{
    /// <inheritdoc cref="TaskbarItemProgressState" />
    public TaskbarItemProgressState Value { get; set; }
}