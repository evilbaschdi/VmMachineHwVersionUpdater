using System.Windows.Shell;

namespace VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

/// <inheritdoc />
/// <summary>
///     Constructor
/// </summary>
/// <param name="updateMachineVersion"></param>
/// <param name="load"></param>
/// <param name="currentItemSource"></param>
/// <param name="taskbarItemProgressState"></param>
/// <param name="reloadDefaultCommand"></param>
public class UpdateAllDefaultCommand(
    [NotNull] IUpdateMachineVersion updateMachineVersion,
    [NotNull] ILoad load,
    [NotNull] ICurrentItemSource currentItemSource,
    [NotNull] ITaskbarItemProgressState taskbarItemProgressState,
    [NotNull] IReloadDefaultCommand reloadDefaultCommand) : IUpdateAllDefaultCommand
{
    private readonly ICurrentItemSource _currentItemSource = currentItemSource ?? throw new ArgumentNullException(nameof(currentItemSource));
    private readonly ILoad _load = load ?? throw new ArgumentNullException(nameof(load));
    private readonly IReloadDefaultCommand _reloadDefaultCommand = reloadDefaultCommand ?? throw new ArgumentNullException(nameof(reloadDefaultCommand));
    private readonly ITaskbarItemProgressState _taskbarItemProgressState = taskbarItemProgressState ?? throw new ArgumentNullException(nameof(taskbarItemProgressState));
    private readonly IUpdateMachineVersion _updateMachineVersion = updateMachineVersion ?? throw new ArgumentNullException(nameof(updateMachineVersion));

    /// <inheritdoc />
    public DefaultCommand DefaultCommandValue
    {
        get
        {
            async void Execute(object _) => await Value();

            return new()
                   {
                       Command = new RelayCommand(Execute)
                   };
        }
    }

    /// <inheritdoc />
    public async Task Value()
    {
        _taskbarItemProgressState.Value = TaskbarItemProgressState.Indeterminate;

        var task = Task.Factory.StartNew(Run);
        await task.ConfigureAwait(true);

        _taskbarItemProgressState.Value = TaskbarItemProgressState.Normal;

        await _reloadDefaultCommand.Value();
    }

    /// <inheritdoc />
    public void Run()
    {
        var version = _load.Value.UpdateAllHwVersion;
        if (!version.HasValue)
        {
            return;
        }

        var innerVersion = Convert.ToInt32(version.Value);
        var localList = _currentItemSource.Value.AsParallel().Where(vm => vm.HwVersion != innerVersion).ToList();
        _updateMachineVersion.RunFor(localList, innerVersion);
    }
}