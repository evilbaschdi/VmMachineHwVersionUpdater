using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using EvilBaschdi.Core;
using JetBrains.Annotations;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc cref="ILoadSearchOsItems" />
    public class LoadSearchOsItems : CachedWritableValue<ObservableCollection<object>>, ILoadSearchOsItems
    {
        private readonly IInit _init;
        private ObservableCollection<object> _searchOsItemCollection = new();

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="init"></param>
        public LoadSearchOsItems([NotNull] IInit init)
        {
            _init = init ?? throw new ArgumentNullException(nameof(init));
        }

        /// <inheritdoc />
        protected override ObservableCollection<object> NonCachedValue
        {
            get
            {
                _searchOsItemCollection.Clear();
                _searchOsItemCollection.Add("(no filter)");
                _searchOsItemCollection.Add(new Separator());
                _init.Load.Value.SearchOsItems.ForEach(x => _searchOsItemCollection.Add(x));
                _searchOsItemCollection.Add(new Separator());
                _init.GuestOsesInUse.Value.ForEach(x => _searchOsItemCollection.Add(x));

                return _searchOsItemCollection;
            }
        }


        /// <inheritdoc />
        protected override void SaveValue(ObservableCollection<object> value)
        {
            _searchOsItemCollection = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}