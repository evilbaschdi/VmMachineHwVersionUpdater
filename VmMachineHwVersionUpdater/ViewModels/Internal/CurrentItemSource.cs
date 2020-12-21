using System;
using System.Collections.Generic;
using EvilBaschdi.Core;
using JetBrains.Annotations;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc cref="ICurrentItemSource" />
    public class CurrentItemSource : CachedWritableValue<List<Machine>>, ICurrentItemSource
    {
        private readonly IInit _init;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="init"></param>
        public CurrentItemSource([NotNull] IInit init)
        {
            _init = init ?? throw new ArgumentNullException(nameof(init));
        }

        /// <inheritdoc />
        protected override List<Machine> NonCachedValue => _init.Load.Value.VmDataGridItemsSource;

        /// <inheritdoc />
        protected override void SaveValue([NotNull] List<Machine> value)
        {
            _init.Load.Value.VmDataGridItemsSource = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}