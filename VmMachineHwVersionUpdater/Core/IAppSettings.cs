// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable once IdentifierTypo
// ReSharper disable IdentifierTypo

using System.Collections.Generic;

namespace VmMachineHwVersionUpdater.Core
{
    /// <summary>
    ///     Wrapper for default settings
    /// </summary>
    public interface IAppSettings
    {
        /// <summary>
        ///     Path of VMware machine archive
        /// </summary>
        List<string> ArchivePath { get; }

        /// <summary>
        ///     Path of VMware machines
        /// </summary>
        List<string> VmPool { get; }
    }
}