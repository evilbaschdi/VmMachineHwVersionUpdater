using System;
using System.Linq;
using EvilBaschdi.Core.Extensions;
using JetBrains.Annotations;

namespace VmMachineHwVersionUpdater.Core.Settings
{
    /// <inheritdoc />
    public class SettingsValid : ISettingsValid
    {
        private readonly IPathSettings _pathSettings;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="pathSettings"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SettingsValid([NotNull] IPathSettings pathSettings)
        {
            _pathSettings = pathSettings ?? throw new ArgumentNullException(nameof(pathSettings));
        }


        /// <inheritdoc />
        public bool Value => _pathSettings.VmPool.GetExistingDirectories().Any();
    }
}