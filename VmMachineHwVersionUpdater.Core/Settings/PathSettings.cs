using Microsoft.Extensions.Configuration;

// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable once IdentifierTypo
// ReSharper disable IdentifierTypo
namespace VmMachineHwVersionUpdater.Core.Settings;

/// <inheritdoc />
public class PathSettings(
    [NotNull] IVmPools vmPools,
    [NotNull] IReplaceUserProfilePlaceholder replaceUserProfilePlaceholder) : IPathSettings
{
    private readonly IVmPools _vmPools = vmPools ?? throw new ArgumentNullException(nameof(vmPools));

    private readonly IReplaceUserProfilePlaceholder _replaceUserProfilePlaceholder =
        replaceUserProfilePlaceholder ?? throw new ArgumentNullException(nameof(replaceUserProfilePlaceholder));

    /// <inheritdoc />
    /// <summary>
    ///     Path of VMware machines
    /// </summary>
    public List<string> VmPool
    {
        get
        {
            var vmPool = _vmPools.Value.GetSection("VmPool").Get<List<string>>();
            return _replaceUserProfilePlaceholder.ValueFor(vmPool);
        }
    }

    /// <inheritdoc />
    /// <summary>
    ///     Path of VMware machine archive
    /// </summary>
    public List<string> ArchivePath
    {
        get
        {
            var archivePath = _vmPools.Value.GetSection("ArchivePath").Get<List<string>>();
            return _replaceUserProfilePlaceholder.ValueFor(archivePath);
        }
    }
}