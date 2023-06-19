namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc />
public class InitReactiveCommands : IInitReactiveCommands
{
    /// <summary>
    ///     Constructor
    /// </summary>
    /// ///
    /// <param name="gotToReactiveCommand"></param>
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
    public InitReactiveCommands(
        [NotNull] IAboutWindowReactiveCommand aboutWindowReactiveCommand,
        [NotNull] IAddEditAnnotationReactiveCommand addEditAnnotationReactiveCommand,
        [NotNull] IArchiveReactiveCommand archiveReactiveCommand,
        [NotNull] ICopyReactiveCommand copyReactiveCommand,
        [NotNull] IDeleteReactiveCommand deleteReactiveCommand,
        [NotNull] IGotToReactiveCommand gotToReactiveCommand,
        [NotNull] IOpenWithCodeReactiveCommand openWithCodeReactiveCommand,
        [NotNull] IReloadReactiveCommand reloadReactiveCommand,
        [NotNull] IRenameReactiveCommand renameReactiveCommand,
        [NotNull] IStartReactiveCommand startReactiveCommand,
        [NotNull] IUpdateAllReactiveCommand updateAllReactiveCommand
    )
    {
        AboutWindowReactiveCommand = aboutWindowReactiveCommand ?? throw new ArgumentNullException(nameof(aboutWindowReactiveCommand));
        AddEditAnnotationReactiveCommand = addEditAnnotationReactiveCommand ?? throw new ArgumentNullException(nameof(addEditAnnotationReactiveCommand));
        ArchiveReactiveCommand = archiveReactiveCommand ?? throw new ArgumentNullException(nameof(archiveReactiveCommand));
        CopyReactiveCommand = copyReactiveCommand ?? throw new ArgumentNullException(nameof(copyReactiveCommand));
        DeleteReactiveCommand = deleteReactiveCommand ?? throw new ArgumentNullException(nameof(deleteReactiveCommand));
        GotToReactiveCommand = gotToReactiveCommand ?? throw new ArgumentNullException(nameof(gotToReactiveCommand));
        OpenWithCodeReactiveCommand = openWithCodeReactiveCommand ?? throw new ArgumentNullException(nameof(openWithCodeReactiveCommand));
        ReloadReactiveCommand = reloadReactiveCommand ?? throw new ArgumentNullException(nameof(reloadReactiveCommand));
        RenameReactiveCommand = renameReactiveCommand ?? throw new ArgumentNullException(nameof(renameReactiveCommand));
        StartReactiveCommand = startReactiveCommand ?? throw new ArgumentNullException(nameof(startReactiveCommand));
        UpdateAllReactiveCommand = updateAllReactiveCommand ?? throw new ArgumentNullException(nameof(updateAllReactiveCommand));
    }

    /// <inheritdoc />
    public IAboutWindowReactiveCommand AboutWindowReactiveCommand { get; set; }

    /// <inheritdoc />
    public IAddEditAnnotationReactiveCommand AddEditAnnotationReactiveCommand { get; set; }

    /// <inheritdoc />
    public IArchiveReactiveCommand ArchiveReactiveCommand { get; set; }

    /// <inheritdoc />
    public ICopyReactiveCommand CopyReactiveCommand { get; set; }

    /// <inheritdoc />
    public IDeleteReactiveCommand DeleteReactiveCommand { get; set; }

    /// <inheritdoc />
    public IGotToReactiveCommand GotToReactiveCommand { get; set; }

    /// <inheritdoc />
    public IOpenWithCodeReactiveCommand OpenWithCodeReactiveCommand { get; set; }

    /// <inheritdoc />
    public IReloadReactiveCommand ReloadReactiveCommand { get; set; }

    /// <inheritdoc />
    public IRenameReactiveCommand RenameReactiveCommand { get; set; }

    /// <inheritdoc />
    public IStartReactiveCommand StartReactiveCommand { get; set; }

    /// <inheritdoc />
    public IUpdateAllReactiveCommand UpdateAllReactiveCommand { get; set; }
}