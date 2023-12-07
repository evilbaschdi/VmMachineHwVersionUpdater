﻿using VmMachineHwVersionUpdater.Core.Commands;

namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IStartReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitRun" />
/// <summary>
///     Constructor
/// </summary>
/// <param name="startCommand"></param>
/// <exception cref="ArgumentNullException"></exception>
public class StartReactiveCommand([NotNull] IStartCommand startCommand) : ReactiveCommandUnitRun, IStartReactiveCommand
{
    private readonly IStartCommand _startCommand = startCommand ?? throw new ArgumentNullException(nameof(startCommand));

    /// <summary>
    ///     Starts VM
    /// </summary>
    public override void Run()
    {
        _startCommand.Run();
    }
}