namespace VmMachineHwVersionUpdater.ViewModels.Internal;

/// <inheritdoc cref="IRun" />
/// <inheritdoc cref="IDialogCoordinatorContext" />
public interface IInitDefaultCommands : IRun, IDialogCoordinatorContext
{
    // ReSharper disable UnusedMemberInSuper.Global
    /// <inheritdoc cref="IAboutWindowClickDefaultCommand" />
    public IAboutWindowClickDefaultCommand AboutWindowClickDefaultCommand { get; set; }

    /// <inheritdoc cref="IAddEditAnnotationDefaultCommand" />
    public IAddEditAnnotationDefaultCommand AddEditAnnotationDefaultCommand { get; set; }

    /// <inheritdoc cref="IArchiveDefaultCommand" />
    public IArchiveDefaultCommand ArchiveDefaultCommand { get; set; }

    /// <inheritdoc cref="ICopyDefaultCommand" />
    public ICopyDefaultCommand CopyDefaultCommand { get; set; }

    /// <inheritdoc cref="IDeleteDefaultCommand" />
    public IDeleteDefaultCommand DeleteDefaultCommand { get; set; }

    /// <inheritdoc cref="IGotToDefaultCommand" />
    public IGotToDefaultCommand GotToDefaultCommand { get; set; }

    /// <inheritdoc cref="IOpenWithCodeDefaultCommand" />
    public IOpenWithCodeDefaultCommand OpenWithCodeDefaultCommand { get; set; }

    /// <inheritdoc cref="IReloadDefaultCommand" />
    public IReloadDefaultCommand ReloadDefaultCommand { get; set; }

    /// <inheritdoc cref="IRenameDefaultCommand" />
    public IRenameDefaultCommand RenameDefaultCommand { get; set; }

    /// <inheritdoc cref="IStartDefaultCommand" />
    public IStartDefaultCommand StartDefaultCommand { get; set; }

    /// <inheritdoc cref="IUpdateAllDefaultCommand" />
    public IUpdateAllDefaultCommand UpdateAllDefaultCommand { get; set; }
    // ReSharper restore UnusedMemberInSuper.Global
}