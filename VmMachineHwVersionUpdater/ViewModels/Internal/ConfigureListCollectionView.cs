using System.ComponentModel;
using System.Windows.Data;
using EvilBaschdi.Core;
using JetBrains.Annotations;
using MahApps.Metro.Controls.Dialogs;
using VmMachineHwVersionUpdater.Core.BasicApplication;
using VmMachineHwVersionUpdater.Core.Models;
using VmMachineHwVersionUpdater.Core.Settings;

namespace VmMachineHwVersionUpdater.ViewModels.Internal;

/// <inheritdoc cref="IConfigureListCollectionView" />
public class ConfigureListCollectionView : CachedWritableValue<ListCollectionView>, IConfigureListCollectionView
{
    [NotNull] private readonly IDialogCoordinator _dialogCoordinator;
    [NotNull] private readonly ILoad _load;
    [NotNull] private readonly ISettingsValid _settingsValid;
    private ListCollectionView _listCollectionView;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="load"></param>
    /// <param name="settingsValid"></param>
    /// <param name="dialogCoordinator"></param>
    public ConfigureListCollectionView([NotNull] ILoad load, [NotNull] ISettingsValid settingsValid, [NotNull] IDialogCoordinator dialogCoordinator)
    {
        _load = load ?? throw new ArgumentNullException(nameof(load));
        _settingsValid = settingsValid ?? throw new ArgumentNullException(nameof(settingsValid));
        _dialogCoordinator = dialogCoordinator ?? throw new ArgumentNullException(nameof(dialogCoordinator));
    }

    /// <inheritdoc />
    protected override ListCollectionView NonCachedValue
    {
        get
        {
            var loadValue = _load.Value;

            if (!_settingsValid.Value)
            {
                _dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "No virtual machines found", "Please verify settings and discs attached");
                return new(new List<Machine>());
            }

            //_dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "Verifying VM pools from settings", $"{loadValue.VmDataGridItemsSource.Count} paths found");

            _listCollectionView = new(loadValue.VmDataGridItemsSource);
            _listCollectionView?.GroupDescriptions?.Add(new PropertyGroupDescription("Directory"));
            _listCollectionView?.SortDescriptions.Add(new("DisplayName", ListSortDirection.Ascending));

            return _listCollectionView;
        }
    }

    /// <inheritdoc />
    public object DialogCoordinatorContext { get; set; }

    /// <inheritdoc />
    protected override void SaveValue(ListCollectionView value)
    {
        _listCollectionView = value ?? throw new ArgumentNullException(nameof(value));
    }
}