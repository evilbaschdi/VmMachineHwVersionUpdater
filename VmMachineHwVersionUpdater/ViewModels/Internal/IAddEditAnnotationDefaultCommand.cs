using System.ComponentModel;
using EvilBaschdi.Core;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;

namespace VmMachineHwVersionUpdater.ViewModels.Internal
{
    /// <inheritdoc cref="IRunDefaultCommand" />
    /// <inheritdoc cref="IRunFor2{TIn1,TIn2}" />
    public interface IAddEditAnnotationDefaultCommand : IRunDefaultCommand, IRunFor2<object, CancelEventArgs>
    {
    }
}