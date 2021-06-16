using System;
using EvilBaschdi.Core;
using JetBrains.Annotations;

namespace VmMachineHwVersionUpdater.Core.Models
{
    /// <inheritdoc cref="ICurrentItem" />
    public class CurrentItem : CachedWritableValue<Machine>, ICurrentItem
    {
        private Machine _machine;

        /// <inheritdoc />
        protected override Machine NonCachedValue => _machine;

        /// <inheritdoc />
        protected override void SaveValue([NotNull] Machine value)
        {
            _machine = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}