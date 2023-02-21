using System.ComponentModel;

namespace VmMachineHwVersionUpdater.ViewModels.Internal;

/// <inheritdoc cref="IDefaultCommandRun" />
/// <inheritdoc cref="IRunFor2{TIn1,TIn2}" />
public interface IAddEditAnnotationDefaultCommand : IDefaultCommandRun, IRunFor2<object, CancelEventArgs>
{
}