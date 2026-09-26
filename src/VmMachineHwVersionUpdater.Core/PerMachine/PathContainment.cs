namespace VmMachineHwVersionUpdater.Core.PerMachine;

internal static class PathContainment
{
    public static bool IsSameOrDescendantOf(string rootPath, string candidatePath)
    {
        var relativePath = Path.GetRelativePath(rootPath, candidatePath);
        return !Path.IsPathRooted(relativePath) &&
               relativePath != ".." &&
               !relativePath.StartsWith($"..{Path.DirectorySeparatorChar}", StringComparison.Ordinal);
    }
}
