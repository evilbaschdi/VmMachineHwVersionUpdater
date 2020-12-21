using System;
using System.ComponentModel;
using System.Windows.Data;
using EvilBaschdi.Core;
using JetBrains.Annotations;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc cref="IConfigureListCollectionView" />
    public class ConfigureListCollectionView : CachedWritableValue<ListCollectionView>, IConfigureListCollectionView
    {
        private readonly IInit _init;
        private readonly SortDescription _sd = new("DisplayName", ListSortDirection.Ascending);
        private ListCollectionView _listCollectionView;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="init"></param>
        public ConfigureListCollectionView([NotNull] IInit init)
        {
            _init = init ?? throw new ArgumentNullException(nameof(init));
        }

        /// <inheritdoc />
        protected override ListCollectionView NonCachedValue
        {
            get
            {
                var loadValue = _init.Load.Value;


                _listCollectionView = new ListCollectionView(loadValue.VmDataGridItemsSource);
                _listCollectionView?.GroupDescriptions?.Add(new PropertyGroupDescription("Directory"));
                _listCollectionView.SortDescriptions.Add(_sd);

                return _listCollectionView;
            }
        }

        /// <inheritdoc />
        protected override void SaveValue(ListCollectionView value)
        {
            _listCollectionView = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}