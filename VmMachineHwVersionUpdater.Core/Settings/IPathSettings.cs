// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable once IdentifierTypo
// ReSharper disable IdentifierTypo

using System.Collections.Generic;

namespace VmMachineHwVersionUpdater.Core.Settings
{
    /// <summary>
    ///     Wrapper for default settings
    /// </summary>
    public interface IPathSettings
    {
        /// <summary>
        ///     Path of VMware machine archive
        /// </summary>
        // ReSharper disable once ReturnTypeCanBeEnumerable.Global
        List<string> ArchivePath { get; }

        /// <summary>
        ///     Path of VMware machines
        /// </summary>
        // ReSharper disable once ReturnTypeCanBeEnumerable.Global
        List<string> VmPool { get; }
    }
}