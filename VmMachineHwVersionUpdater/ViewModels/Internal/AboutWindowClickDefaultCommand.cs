using EvilBaschdi.CoreExtended.Controls.About;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using Microsoft.Extensions.DependencyInjection;

namespace VmMachineHwVersionUpdater.ViewModels.Internal;

/// <inheritdoc />
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class AboutWindowClickDefaultCommand : IAboutWindowClickDefaultCommand
{
    /// <inheritdoc />
    public void Run()
    {
        var aboutWindow = App.ServiceProvider.GetRequiredService<AboutWindow>();
        aboutWindow.ShowDialog();
    }

    /// <inheritdoc />
    public DefaultCommand DefaultCommandValue => new()
                                                 {
                                                     Command = new RelayCommand(_ => Run())
                                                 };
}