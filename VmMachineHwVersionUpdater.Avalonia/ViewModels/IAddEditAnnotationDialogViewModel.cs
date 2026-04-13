namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <summary />
public interface IAddEditAnnotationDialogViewModel
{
    /// <summary />
    // ReSharper disable once UnusedMember.Global
    public string Annotation { get; set; }

    /// <summary>
    ///     Binding
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public Machine SelectedMachine { get; set; }
}