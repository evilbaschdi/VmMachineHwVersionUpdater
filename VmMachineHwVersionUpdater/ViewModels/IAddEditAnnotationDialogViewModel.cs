using VmMachineHwVersionUpdater.Core.Models;

namespace VmMachineHwVersionUpdater.ViewModels;

/// <summary />
public interface IAddEditAnnotationDialogViewModel
{
    /// <summary />
    // ReSharper disable once UnusedMember.Global
    public string AnnotationText { get; set; }

    /// <summary>
    ///     Binding
    /// </summary>

    // ReSharper disable once UnusedMember.Global
    public Machine SelectedMachine { get; set; }
}