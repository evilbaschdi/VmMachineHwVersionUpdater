namespace VmMachineHwVersionUpdater.Core.BasicApplication;

/// <inheritdoc cref="IResolveMachinePoolPath" />
public class ResolveMachinePoolPath(
    [NotNull] IPathSettings pathSettings) : IResolveMachinePoolPath
{
    private readonly IPathSettings
        _pathSettings = pathSettings ?? throw new ArgumentNullException(nameof(pathSettings));

    /// <inheritdoc />
    public string ValueFor(string filePath)
    {
        ArgumentNullException.ThrowIfNull(filePath);

        var allPaths = _pathSettings.VmPool.Concat(_pathSettings.ArchivePath);

        return allPaths
               .Where(pool => filePath.StartsWith(pool, StringComparison.OrdinalIgnoreCase))
               .OrderByDescending(pool => pool.Length)
               .FirstOrDefault();
    }
}