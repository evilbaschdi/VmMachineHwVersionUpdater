namespace VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

/// <inheritdoc />
public class InitDefaultCommands : IInitDefaultCommands
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public InitDefaultCommands([NotNull] IAboutWindowClickDefaultCommand aboutWindowClickDefaultCommand,
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
    )
    {
        AboutWindowClickDefaultCommand = aboutWindowClickDefaultCommand;
        ArchiveDefaultCommand = archiveDefaultCommand;
        CopyDefaultCommand = copyDefaultCommand;
        OpenWithCodeDefaultCommand = openWithCodeDefaultCommand;
        AddEditAnnotationDefaultCommand = addEditAnnotationDefaultCommand;
        DeleteDefaultCommand = deleteDefaultCommand;
        GotToDefaultCommand = gotToDefaultCommand;
        ReloadDefaultCommand = reloadDefaultCommand;
        RenameDefaultCommand = renameDefaultCommand;
        StartDefaultCommand = startDefaultCommand;
        UpdateAllDefaultCommand = updateAllDefaultCommand;
    }

    /// <inheritdoc />
    public IAboutWindowClickDefaultCommand AboutWindowClickDefaultCommand { get; set; }

    /// <inheritdoc />
    public IArchiveDefaultCommand ArchiveDefaultCommand { get; set; }

    /// <inheritdoc />
    public ICopyDefaultCommand CopyDefaultCommand { get; set; }

    /// <inheritdoc />
    public IOpenWithCodeDefaultCommand OpenWithCodeDefaultCommand { get; set; }

    /// <inheritdoc />
    public IAddEditAnnotationDefaultCommand AddEditAnnotationDefaultCommand { get; set; }

    /// <inheritdoc />
    public IDeleteDefaultCommand DeleteDefaultCommand { get; set; }

    /// <inheritdoc />
    public IGotToDefaultCommand GotToDefaultCommand { get; set; }

    /// <inheritdoc />
    public IReloadDefaultCommand ReloadDefaultCommand { get; set; }

    /// <inheritdoc />
    public IRenameDefaultCommand RenameDefaultCommand { get; set; }

    /// <inheritdoc />
    public IStartDefaultCommand StartDefaultCommand { get; set; }

    /// <inheritdoc />
    public IUpdateAllDefaultCommand UpdateAllDefaultCommand { get; set; }

    /// <inheritdoc />
    public object DialogCoordinatorContext { get; set; }
}