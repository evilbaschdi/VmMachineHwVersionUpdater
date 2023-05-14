using MahApps.Metro.Controls.Dialogs;

namespace VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

/// <inheritdoc cref="ICurrentItemSource" />
public class CurrentItemSource : CachedWritableValue<List<Machine>>, ICurrentItemSource
{
    private readonly IDialogCoordinator _dialogCoordinator;
    private readonly ILoad _load;
    private readonly ISettingsValid _settingsValid;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="load"></param>
    /// <param name="settingsValid"></param>
    /// <param name="dialogCoordinator"></param>
    public CurrentItemSource([NotNull] ILoad load, [NotNull] ISettingsValid settingsValid, [NotNull] IDialogCoordinator dialogCoordinator)
    {
        _load = load ?? throw new ArgumentNullException(nameof(load));
        _settingsValid = settingsValid ?? throw new ArgumentNullException(nameof(settingsValid));
        _dialogCoordinator = dialogCoordinator ?? throw new ArgumentNullException(nameof(dialogCoordinator));
    }

    /// <inheritdoc />
    protected override List<Machine> NonCachedValue
    {
        get
        {
            if (!_settingsValid.Value)
            {
                _dialogCoordinator.ShowMessageAsync(this, "No virtual machines found", "Please verify settings and discs attached");
                return new();
            }

            var itemsSource = _load.Value.VmDataGridItemsSource;

            _dialogCoordinator.ShowMessageAsync(this, "Verifying VM pools from settings", $"{itemsSource.Count} paths found");

            return _load.Value.VmDataGridItemsSource;
        }
    }

    /// <inheritdoc />
    protected override void SaveValue([NotNull] List<Machine> value)
    {
        _load.Value.VmDataGridItemsSource = value ?? throw new ArgumentNullException(nameof(value));
    }
}