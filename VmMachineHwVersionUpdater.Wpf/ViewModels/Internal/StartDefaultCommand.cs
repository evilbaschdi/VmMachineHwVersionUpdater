﻿using VmMachineHwVersionUpdater.Core.Commands;

namespace VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

/// <inheritdoc />
public class StartDefaultCommand(
    [NotNull] IStartCommand startCommand) : IStartDefaultCommand
{
    private readonly IStartCommand _startCommand = startCommand ?? throw new ArgumentNullException(nameof(startCommand));

    /// <inheritdoc />
    public DefaultCommand DefaultCommandValue => new()
                                                 {
                                                     Command = new RelayCommand(_ => Run())
                                                 };

    /// <inheritdoc />
    public void Run()
    {
        _startCommand.Run();
    }
}