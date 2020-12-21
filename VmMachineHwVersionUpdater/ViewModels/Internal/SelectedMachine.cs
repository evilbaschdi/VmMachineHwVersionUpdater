using System;
using EvilBaschdi.Core;
using JetBrains.Annotations;
using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc cref="ISelectedMachine" />
    public class SelectedMachine : CachedWritableValue<Machine>, ISelectedMachine
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