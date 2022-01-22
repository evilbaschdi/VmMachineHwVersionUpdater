using System;
using EvilBaschdi.CoreExtended.Controls.About;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using JetBrains.Annotations;

namespace VmMachineHwVersionUpdater.ViewModels.Internal;

/// <inheritdoc />
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class AboutWindowClickDefaultCommand : IAboutWindowClickDefaultCommand
{
    private readonly AboutWindow _aboutWindow;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="aboutWindow"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public AboutWindowClickDefaultCommand([NotNull] AboutWindow aboutWindow)
    {
        _aboutWindow = aboutWindow ?? throw new ArgumentNullException(nameof(aboutWindow));
    }

    /// <inheritdoc />
    public void Run()
    {
        _aboutWindow.ShowDialog();
    }

    /// <inheritdoc />
    public DefaultCommand Value => new()
                                   {
                                       Command = new RelayCommand(_ => Run())
                                   };

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// </summary>
    /// <param name="disposing"></param>
    // ReSharper disable once VirtualMemberNeverOverridden.Global
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
        {
            return;
        }

        _aboutWindow.DataContext = null;
        _aboutWindow.Close();
    }
}