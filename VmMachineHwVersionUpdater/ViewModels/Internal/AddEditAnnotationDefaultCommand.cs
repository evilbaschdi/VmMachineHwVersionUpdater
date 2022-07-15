﻿using System.ComponentModel;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using Microsoft.Extensions.DependencyInjection;

namespace VmMachineHwVersionUpdater.ViewModels.Internal;

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
        if (sender == null)
        {
            throw new ArgumentNullException(nameof(sender));
        }

        if (args == null)
        {
            throw new ArgumentNullException(nameof(args));
        }

        await _reloadDefaultCommand.Value();
    }
}