namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IUpdateAllReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
public class UpdateAllReactiveCommand : ReactiveCommandUnitRun, IUpdateAllReactiveCommand
{
    [NotNull] private readonly IUpdateMachineVersion _updateMachineVersion;
    [NotNull] private readonly ILoad _load;
    [NotNull] private readonly IReloadReactiveCommand _reloadReactiveCommand;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="updateMachineVersion"></param>
    /// <param name="load"></param>
    /// <param name="reloadReactiveCommand"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public UpdateAllReactiveCommand(
        [NotNull] IUpdateMachineVersion updateMachineVersion,
        [NotNull] ILoad load,
        [NotNull] IReloadReactiveCommand reloadReactiveCommand)
    {
        _updateMachineVersion = updateMachineVersion ?? throw new ArgumentNullException(nameof(updateMachineVersion));
        _load = load ?? throw new ArgumentNullException(nameof(load));
        _reloadReactiveCommand = reloadReactiveCommand ?? throw new ArgumentNullException(nameof(reloadReactiveCommand));
    }

    /// <inheritdoc />
    public override void Run()
    {
        var version = _load.Value.UpdateAllHwVersion;
        if (!version.HasValue)
        {
            return;
        }

        var innerVersion = Convert.ToInt32(version.Value);
        var localList = _load.Value.VmDataGridItemsSource.AsParallel().Where(vm => vm.HwVersion != innerVersion).ToList();
        _updateMachineVersion.RunFor(localList, innerVersion);

        _reloadReactiveCommand.Run();
    }
}