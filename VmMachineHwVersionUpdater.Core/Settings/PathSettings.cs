using Microsoft.Extensions.Configuration;

// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable once IdentifierTypo
// ReSharper disable IdentifierTypo
namespace VmMachineHwVersionUpdater.Core.Settings;

/// <inheritdoc />
public class PathSettings : IPathSettings
{
    private readonly IVmPools _vmPools;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="vmPools"></param>
    public PathSettings(IVmPools vmPools)
    {
        _vmPools = vmPools ?? throw new ArgumentNullException(nameof(vmPools));
    }

    /// <inheritdoc />
    /// <summary>
    ///     Path of VMware machines
    /// </summary>

    public List<string> VmPool => _vmPools.Value.GetSection("VmPool").Get<List<string>>();

    /// <inheritdoc />
    /// <summary>
    ///     Path of VMware machine archive
    /// </summary>
    public List<string> ArchivePath => _vmPools.Value.GetSection("ArchivePath").Get<List<string>>();
}