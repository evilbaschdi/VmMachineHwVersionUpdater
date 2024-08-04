namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <summary>
/// </summary>
public interface IInitReactiveCommands
{
    /// <inheritdoc cref="IAboutWindowReactiveCommand" />
    // ReSharper disable UnusedMemberInSuper.Global
    // ReSharper disable UnusedMember.Global
    public IAboutWindowReactiveCommand AboutWindowReactiveCommand { get; set; }

    /// <inheritdoc cref="IAddEditAnnotationReactiveCommand" />
    public IAddEditAnnotationReactiveCommand AddEditAnnotationReactiveCommand { get; set; }

    /// <inheritdoc cref="IArchiveReactiveCommand" />
    public IArchiveReactiveCommand ArchiveReactiveCommand { get; set; }

    /// <inheritdoc cref="ICopyReactiveCommand" />
    public ICopyReactiveCommand CopyReactiveCommand { get; set; }

    /// <inheritdoc cref="IDeleteReactiveCommand" />
    public IDeleteReactiveCommand DeleteReactiveCommand { get; set; }

    /// <inheritdoc cref="IGoToReactiveCommand" />
    public IGoToReactiveCommand GoToReactiveCommand { get; set; }

    /// <inheritdoc cref="IOpenWithCodeReactiveCommand" />
    public IOpenWithCodeReactiveCommand OpenWithCodeReactiveCommand { get; set; }

    /// <inheritdoc cref="IReloadReactiveCommand" />
    public IReloadReactiveCommand ReloadReactiveCommand { get; set; }

    /// <inheritdoc cref="IRenameReactiveCommand" />
    public IRenameReactiveCommand RenameReactiveCommand { get; set; }

    /// <inheritdoc cref="IStartReactiveCommand" />
    public IStartReactiveCommand StartReactiveCommand { get; set; }

    /// <inheritdoc cref="IUpdateAllReactiveCommand" />
    public IUpdateAllReactiveCommand UpdateAllReactiveCommand { get; set; }
    // ReSharper restore UnusedMember.Global
    // ReSharper restore UnusedMemberInSuper.Global
}