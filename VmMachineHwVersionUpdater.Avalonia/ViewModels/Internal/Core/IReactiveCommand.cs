using ReactiveUI;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal.Core;

/// <summary>IReactiveCommand</summary>
public interface IReactiveCommand<TParam, TResult>
{
    /// <inheritdoc cref="ReactiveCommand" />
    ReactiveCommand<TParam, TResult> ReactiveCommandValue { get; }
}