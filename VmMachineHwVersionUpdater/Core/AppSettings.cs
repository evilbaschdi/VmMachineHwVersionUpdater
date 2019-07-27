using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

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
        /// <inheritdoc />
        /// <summary>
        ///     Path of VMware machines
        /// </summary>

        public List<string> VmPool => VmPools.AppSetting.GetSection("VmPool").Get<List<string>>();

        /// <inheritdoc />
        /// <summary>
        ///     Path of VMware machine archive
        /// </summary>
        public List<string> ArchivePath => VmPools.AppSetting.GetSection("ArchivePath").Get<List<string>>();
    }
}