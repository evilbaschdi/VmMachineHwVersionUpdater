namespace VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

/// <inheritdoc />
public class InitDefaultCommands(
    [NotNull] IAboutWindowClickDefaultCommand aboutWindowClickDefaultCommand,
    [NotNull] IArchiveDefaultCommand archiveDefaultCommand,
    [NotNull] ICopyDefaultCommand copyDefaultCommand,
    [NotNull] IOpenWithCodeDefaultCommand openWithCodeDefaultCommand,
    [NotNull] IAddEditAnnotationDefaultCommand addEditAnnotationDefaultCommand,
    [NotNull] IDeleteDefaultCommand deleteDefaultCommand,
    [NotNull] IGotToDefaultCommand gotToDefaultCommand,
    [NotNull] IReloadDefaultCommand reloadDefaultCommand,
    [NotNull] IRenameDefaultCommand renameDefaultCommand,
    [NotNull] IStartDefaultCommand startDefaultCommand,
    [NotNull] IUpdateAllDefaultCommand updateAllDefaultCommand
) : IInitDefaultCommands
{
    /// <inheritdoc />
    public IAboutWindowClickDefaultCommand AboutWindowClickDefaultCommand { get; set; } = aboutWindowClickDefaultCommand;

    /// <inheritdoc />
    public IArchiveDefaultCommand ArchiveDefaultCommand { get; set; } = archiveDefaultCommand;

    /// <inheritdoc />
    public ICopyDefaultCommand CopyDefaultCommand { get; set; } = copyDefaultCommand;

    /// <inheritdoc />
    public IOpenWithCodeDefaultCommand OpenWithCodeDefaultCommand { get; set; } = openWithCodeDefaultCommand;

    /// <inheritdoc />
    public IAddEditAnnotationDefaultCommand AddEditAnnotationDefaultCommand { get; set; } = addEditAnnotationDefaultCommand;

    /// <inheritdoc />
    public IDeleteDefaultCommand DeleteDefaultCommand { get; set; } = deleteDefaultCommand;

    /// <inheritdoc />
    public IGotToDefaultCommand GotToDefaultCommand { get; set; } = gotToDefaultCommand;

    /// <inheritdoc />
    public IReloadDefaultCommand ReloadDefaultCommand { get; set; } = reloadDefaultCommand;

    /// <inheritdoc />
    public IRenameDefaultCommand RenameDefaultCommand { get; set; } = renameDefaultCommand;

    /// <inheritdoc />
    public IStartDefaultCommand StartDefaultCommand { get; set; } = startDefaultCommand;

    /// <inheritdoc />
    public IUpdateAllDefaultCommand UpdateAllDefaultCommand { get; set; } = updateAllDefaultCommand;

    /// <inheritdoc />
    public object DialogCoordinatorContext { get; set; }
}