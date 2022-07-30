using EvilBaschdi.CoreExtended.Controls.About;

namespace VmMachineHwVersionUpdater.ViewModels.Internal;

/// <inheritdoc />
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global
public class AboutWindowClickDefaultCommand : IAboutWindowClickDefaultCommand
{
    /// <inheritdoc />
    public DefaultCommand DefaultCommandValue => new()
                                                 {
                                                     Command = new RelayCommand(_ => Run())
                                                 };

    /// <inheritdoc />
    public void Run()
    {
        var aboutWindow = App.ServiceProvider.GetRequiredService<AboutWindow>();
        aboutWindow.ShowDialog();
    }
}