﻿using System.Collections.Concurrent;
using MahApps.Metro.Controls.Dialogs;

namespace VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

/// <inheritdoc cref="ICurrentItemSource" />
public class CurrentItemSource(
    [NotNull] ILoad load,
    [NotNull] ISettingsValid settingsValid,
    [NotNull] IDialogCoordinator dialogCoordinator) : CachedWritableValue<ConcurrentBag<Machine>>, ICurrentItemSource
{
    private readonly IDialogCoordinator _dialogCoordinator = dialogCoordinator ?? throw new ArgumentNullException(nameof(dialogCoordinator));
    private readonly ILoad _load = load ?? throw new ArgumentNullException(nameof(load));
    private readonly ISettingsValid _settingsValid = settingsValid ?? throw new ArgumentNullException(nameof(settingsValid));

    /// <inheritdoc />
    protected override ConcurrentBag<Machine> NonCachedValue
    {
        get
        {
            if (!_settingsValid.Value)
            {
                _dialogCoordinator.ShowMessageAsync(this, "No virtual machines found", "Please verify settings and discs attached");
                return [];
            }

            var itemsSource = _load.Value.VmDataGridItemsSource;

            _dialogCoordinator.ShowMessageAsync(this, "Verifying VM pools from settings", $"{itemsSource.Count} paths found");

            return _load.Value.VmDataGridItemsSource;
        }
    }

    /// <inheritdoc />
    protected override void SaveValue([NotNull] ConcurrentBag<Machine> value)
    {
        _load.Value.VmDataGridItemsSource = value ?? throw new ArgumentNullException(nameof(value));
    }
}