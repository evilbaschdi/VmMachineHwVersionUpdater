using EvilBaschdi.Core.Extensions;

namespace VmMachineHwVersionUpdater.Core.Settings;

/// <inheritdoc />
public class SettingsValid(
    [NotNull] IPathSettings pathSettings) : ISettingsValid
{
    private readonly IPathSettings _pathSettings = pathSettings ?? throw new ArgumentNullException(nameof(pathSettings));

    /// <inheritdoc />
    public bool Value => _pathSettings.VmPool.GetExistingDirectories().Any();
}