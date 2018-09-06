using System;
using System.IO;
using EvilBaschdi.CoreExtended.AppHelpers;

// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable once IdentifierTypo
// ReSharper disable IdentifierTypo
namespace VmMachineHwVersionUpdater.Core
{
    /// <inheritdoc />
    /// <summary>
    ///     Wrapper for Default Settings.
    /// </summary>
    public class AppSettings : IAppSettings
    {
        private readonly IAppSettingsBase _appSettingsBase;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="applicationSettingsBase"></param>
        public AppSettings(IAppSettingsBase applicationSettingsBase)
        {
            _appSettingsBase = applicationSettingsBase ??
                               throw new ArgumentNullException(nameof(applicationSettingsBase));
        }

        /// <inheritdoc />
        /// <summary>
        ///     Path of VMware machines
        /// </summary>

        public string VMwarePool
        {
            get => _appSettingsBase.Get("VMwarePool", "");

            set => _appSettingsBase.Set("VMwarePool", value);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Path of VMware machine archive
        /// </summary>
        public string ArchivePath
        {
            get => _appSettingsBase.Get("ArchivePath", !string.IsNullOrWhiteSpace(VMwarePool)
                ? Path.Combine(VMwarePool, "_archive")
                : "");
            set => _appSettingsBase.Set("ArchivePath", value);
        }
    }
}