namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IUpdateAllReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitTask" />
public class UpdateAllReactiveCommand(
    [NotNull] IUpdateMachineVersion updateMachineVersion,
    [NotNull] ILoad load,
    [NotNull] IReloadReactiveCommand reloadReactiveCommand) : ReactiveCommandUnitTask, IUpdateAllReactiveCommand
{
    private readonly IUpdateMachineVersion _updateMachineVersion = updateMachineVersion ?? throw new ArgumentNullException(nameof(updateMachineVersion));
    private readonly ILoad _load = load ?? throw new ArgumentNullException(nameof(load));
    private readonly IReloadReactiveCommand _reloadReactiveCommand = reloadReactiveCommand ?? throw new ArgumentNullException(nameof(reloadReactiveCommand));

    /// <inheritdoc />
    public override async Task RunAsync()
    {
        var version = _load.Value.UpdateAllHwVersion;
        if (!version.HasValue)
        {
            return;
        }

        var innerVersion = Convert.ToInt32(version.Value);
        var localList = _load.Value.VmDataGridItemsSource.AsParallel().Where(vm => vm.HwVersion != innerVersion);
        _updateMachineVersion.RunFor(localList, innerVersion);

        await _reloadReactiveCommand.RunAsync();
    }
}