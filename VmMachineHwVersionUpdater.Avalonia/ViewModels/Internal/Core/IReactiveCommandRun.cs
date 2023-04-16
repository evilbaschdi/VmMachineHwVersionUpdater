using EvilBaschdi.Core;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal.Core;

/// <inheritdoc cref="IRun" />
/// <inheritdoc cref="IReactiveCommand{TParam,TResult}" />
public interface IReactiveCommandRun<TParam, TResult> : IReactiveCommand<TParam, TResult>, IRun
{
}