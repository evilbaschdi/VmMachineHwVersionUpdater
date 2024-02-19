namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc />
/// <summary>
///     Constructor
/// </summary>
/// ///
/// <param name="goToReactiveCommand"></param>
/// <param name="openWithCodeReactiveCommand"></param>
/// <param name="renameReactiveCommand"></param>
/// <param name="startReactiveCommand"></param>
/// <param name="aboutWindowReactiveCommand"></param>
/// <param name="addEditAnnotationReactiveCommand"></param>
/// <param name="archiveReactiveCommand"></param>
/// <param name="copyReactiveCommand"></param>
/// <param name="deleteReactiveCommand"></param>
/// <param name="reloadReactiveCommand"></param>
/// <param name="updateAllReactiveCommand"></param>
/// <exception cref="ArgumentNullException"></exception>
public class InitReactiveCommands(
    [NotNull] IAboutWindowReactiveCommand aboutWindowReactiveCommand,
    [NotNull] IAddEditAnnotationReactiveCommand addEditAnnotationReactiveCommand,
    [NotNull] IArchiveReactiveCommand archiveReactiveCommand,
    [NotNull] ICopyReactiveCommand copyReactiveCommand,
    [NotNull] IDeleteReactiveCommand deleteReactiveCommand,
    [NotNull] IGoToReactiveCommand goToReactiveCommand,
    [NotNull] IOpenWithCodeReactiveCommand openWithCodeReactiveCommand,
    [NotNull] IReloadReactiveCommand reloadReactiveCommand,
    [NotNull] IRenameReactiveCommand renameReactiveCommand,
    [NotNull] IStartReactiveCommand startReactiveCommand,
    [NotNull] IUpdateAllReactiveCommand updateAllReactiveCommand
) : IInitReactiveCommands
{
    /// <inheritdoc />
    public IAboutWindowReactiveCommand AboutWindowReactiveCommand { get; set; } = aboutWindowReactiveCommand ?? throw new ArgumentNullException(nameof(aboutWindowReactiveCommand));

    /// <inheritdoc />
    public IAddEditAnnotationReactiveCommand AddEditAnnotationReactiveCommand { get; set; } =
        addEditAnnotationReactiveCommand ?? throw new ArgumentNullException(nameof(addEditAnnotationReactiveCommand));

    /// <inheritdoc />
    public IArchiveReactiveCommand ArchiveReactiveCommand { get; set; } = archiveReactiveCommand ?? throw new ArgumentNullException(nameof(archiveReactiveCommand));

    /// <inheritdoc />
    public ICopyReactiveCommand CopyReactiveCommand { get; set; } = copyReactiveCommand ?? throw new ArgumentNullException(nameof(copyReactiveCommand));

    /// <inheritdoc />
    public IDeleteReactiveCommand DeleteReactiveCommand { get; set; } = deleteReactiveCommand ?? throw new ArgumentNullException(nameof(deleteReactiveCommand));

    /// <inheritdoc />
    public IGoToReactiveCommand GoToReactiveCommand { get; set; } = goToReactiveCommand ?? throw new ArgumentNullException(nameof(goToReactiveCommand));

    /// <inheritdoc />
    public IOpenWithCodeReactiveCommand OpenWithCodeReactiveCommand { get; set; } =
        openWithCodeReactiveCommand ?? throw new ArgumentNullException(nameof(openWithCodeReactiveCommand));

    /// <inheritdoc />
    public IReloadReactiveCommand ReloadReactiveCommand { get; set; } = reloadReactiveCommand ?? throw new ArgumentNullException(nameof(reloadReactiveCommand));

    /// <inheritdoc />
    public IRenameReactiveCommand RenameReactiveCommand { get; set; } = renameReactiveCommand ?? throw new ArgumentNullException(nameof(renameReactiveCommand));

    /// <inheritdoc />
    public IStartReactiveCommand StartReactiveCommand { get; set; } = startReactiveCommand ?? throw new ArgumentNullException(nameof(startReactiveCommand));

    /// <inheritdoc />
    public IUpdateAllReactiveCommand UpdateAllReactiveCommand { get; set; } = updateAllReactiveCommand ?? throw new ArgumentNullException(nameof(updateAllReactiveCommand));
}