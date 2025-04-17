namespace VmMachineHwVersionUpdater.Core.Settings;

/// <inheritdoc />
public class ReplaceUserProfilePlaceholder : IReplaceUserProfilePlaceholder
{
    /// <inheritdoc />
    public List<string> ValueFor([NotNull] List<string> paths)
    {
        ArgumentNullException.ThrowIfNull(paths);

        // Get the home directory dynamically
        var userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

        return paths.Select(path => path.Replace("{UserProfile}", userProfile)).ToList();
    }
}