﻿using System.ComponentModel;
using VmMachineHwVersionUpdater.Wpf.Views;

namespace VmMachineHwVersionUpdater.Wpf.ViewModels.Internal;

/// <inheritdoc />
public class AddEditAnnotationDefaultCommand : IAddEditAnnotationDefaultCommand
{
    private readonly IReloadDefaultCommand _reloadDefaultCommand;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="reloadDefaultCommand"></param>
    public AddEditAnnotationDefaultCommand([NotNull] IReloadDefaultCommand reloadDefaultCommand)
    {
        _reloadDefaultCommand = reloadDefaultCommand ?? throw new ArgumentNullException(nameof(reloadDefaultCommand));
    }

    /// <inheritdoc />
    public DefaultCommand DefaultCommandValue => new()
                                                 {
                                                     Command = new RelayCommand(_ => Run())
                                                 };

    /// <inheritdoc />
    public void Run()
    {
        var addEditAnnotationDialog = App.ServiceProvider.GetRequiredService<AddEditAnnotationDialog>();
        addEditAnnotationDialog.Closing += RunFor;
        addEditAnnotationDialog.ShowDialog();
    }

    /// <inheritdoc />
    public async void RunFor([NotNull] object sender, [NotNull] CancelEventArgs args)
    {
        ArgumentNullException.ThrowIfNull(sender);
        ArgumentNullException.ThrowIfNull(args);

        await _reloadDefaultCommand.Value();
    }
}