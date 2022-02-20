using JetBrains.Annotations;

namespace VmMachineHwVersionUpdater.ViewModels.Internal;

/// <inheritdoc />
public class InitDefaultCommands : IInitDefaultCommands
{
    private readonly IAboutWindowClickDefaultCommand _aboutWindowClickDefaultCommand;
    private readonly IAddEditAnnotationDefaultCommand _addEditAnnotationDefaultCommand;
    private readonly IArchiveDefaultCommand _archiveDefaultCommand;
    private readonly ICopyDefaultCommand _copyDefaultCommand;
    private readonly IDeleteDefaultCommand _deleteDefaultCommand;
    private readonly IGotToDefaultCommand _gotToDefaultCommand;
    private readonly IOpenWithCodeDefaultCommand _openWithCodeDefaultCommand;
    private readonly IReloadDefaultCommand _reloadDefaultCommand;
    private readonly IStartDefaultCommand _startDefaultCommand;
    private readonly IUpdateAllDefaultCommand _updateAllDefaultCommand;

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
                               [NotNull] IStartDefaultCommand startDefaultCommand,
                               [NotNull] IUpdateAllDefaultCommand updateAllDefaultCommand
    )
    {
        _aboutWindowClickDefaultCommand = aboutWindowClickDefaultCommand ?? throw new ArgumentNullException(nameof(aboutWindowClickDefaultCommand));
        _archiveDefaultCommand = archiveDefaultCommand ?? throw new ArgumentNullException(nameof(archiveDefaultCommand));
        _copyDefaultCommand = copyDefaultCommand ?? throw new ArgumentNullException(nameof(copyDefaultCommand));
        _openWithCodeDefaultCommand = openWithCodeDefaultCommand ?? throw new ArgumentNullException(nameof(openWithCodeDefaultCommand));
        _addEditAnnotationDefaultCommand = addEditAnnotationDefaultCommand ?? throw new ArgumentNullException(nameof(addEditAnnotationDefaultCommand));
        _deleteDefaultCommand = deleteDefaultCommand ?? throw new ArgumentNullException(nameof(deleteDefaultCommand));
        _gotToDefaultCommand = gotToDefaultCommand ?? throw new ArgumentNullException(nameof(gotToDefaultCommand));
        _reloadDefaultCommand = reloadDefaultCommand ?? throw new ArgumentNullException(nameof(reloadDefaultCommand));
        _startDefaultCommand = startDefaultCommand ?? throw new ArgumentNullException(nameof(startDefaultCommand));
        _updateAllDefaultCommand = updateAllDefaultCommand ?? throw new ArgumentNullException(nameof(updateAllDefaultCommand));
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
    public IStartDefaultCommand StartDefaultCommand { get; set; }

    /// <inheritdoc />
    public IUpdateAllDefaultCommand UpdateAllDefaultCommand { get; set; }

    /// <inheritdoc />
    public object DialogCoordinatorContext { get; set; }

    /// <inheritdoc />
    public void Run()
    {
        AboutWindowClickDefaultCommand = _aboutWindowClickDefaultCommand;
        ArchiveDefaultCommand = _archiveDefaultCommand;
        CopyDefaultCommand = _copyDefaultCommand;
        OpenWithCodeDefaultCommand = _openWithCodeDefaultCommand;
        AddEditAnnotationDefaultCommand = _addEditAnnotationDefaultCommand;
        ReloadDefaultCommand = _reloadDefaultCommand;
        DeleteDefaultCommand = _deleteDefaultCommand;
        GotToDefaultCommand = _gotToDefaultCommand;
        StartDefaultCommand = _startDefaultCommand;
        UpdateAllDefaultCommand = _updateAllDefaultCommand;

        ArchiveDefaultCommand.DialogCoordinatorContext = DialogCoordinatorContext;
        CopyDefaultCommand.DialogCoordinatorContext = DialogCoordinatorContext;
        DeleteDefaultCommand.DialogCoordinatorContext = DialogCoordinatorContext;
        ReloadDefaultCommand.DialogCoordinatorContext = DialogCoordinatorContext;
    }
}