using System.Reactive;
using ReactiveUI;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal.Core;

/// <inheritdoc />
public abstract class ReactiveCommandUnitRun : IReactiveCommandUnitRun
{
    /// <inheritdoc />
    public ReactiveCommand<Unit, Unit> ReactiveCommandValue => ReactiveCommand.Create(Run);

    /// <inheritdoc />
    public virtual void Run()
    {
        throw new NotImplementedException();
    }
}