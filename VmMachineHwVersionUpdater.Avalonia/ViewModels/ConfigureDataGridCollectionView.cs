using System.Collections;
using System.ComponentModel;
using Avalonia.Collections;
using EvilBaschdi.Core;
using JetBrains.Annotations;
using VmMachineHwVersionUpdater.Core.BasicApplication;
using VmMachineHwVersionUpdater.Core.Models;
using VmMachineHwVersionUpdater.Core.Settings;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels
{
    /// <inheritdoc cref="IConfigureDataGridCollectionView" />
    public class ConfigureDataGridCollectionView : CachedWritableValue<DataGridCollectionView>, IConfigureDataGridCollectionView
    {
        private readonly IComparer _comparer;
        [NotNull] private readonly ILoad _load;
        [NotNull] private readonly ISettingsValid _settingsValid;

        private DataGridCollectionView _dataGridCollectionView;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="load"></param>
        /// <param name="settingsValid"></param>
        /// <param name="comparer"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConfigureDataGridCollectionView([NotNull] ILoad load,
                                               [NotNull] ISettingsValid settingsValid,
                                               [NotNull] IComparer comparer)
        {
            _load = load ?? throw new ArgumentNullException(nameof(load));
            _settingsValid = settingsValid ?? throw new ArgumentNullException(nameof(settingsValid));
            _comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }

        /// <inheritdoc />
        protected override DataGridCollectionView NonCachedValue
        {
            get
            {
                if (_load.Value.VmDataGridItemsSource == null)
                {
                    return new DataGridCollectionView(new List<Machine>());
                }

                var loadValue = _load.Value;

                if (!_settingsValid.Value)
                {
                    //_dialogCoordinator.ShowMessageAsync(DialogCoordinatorContext, "No virtual machines found", "Please verify settings and discs attached");
                    return new(new List<Machine>());
                }

                _dataGridCollectionView = new DataGridCollectionView(loadValue.VmDataGridItemsSource)
                                          {
                                              GroupDescriptions =
                                              {
                                                  new DataGridPathGroupDescription("Directory")
                                              }
                                          };

                _dataGridCollectionView.SortDescriptions.Add(new DataGridComparerSortDescription(_comparer, ListSortDirection.Ascending));

                return _dataGridCollectionView;
            }
        }

        /// <inheritdoc />
        protected override void SaveValue(DataGridCollectionView value)
        {
            _dataGridCollectionView = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}