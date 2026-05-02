using Avalonia;
using Avalonia.Themes.Fluent;
using VmMachineHwVersionUpdater.Avalonia.Tests;

[assembly: AvaloniaTestApplication(typeof(TestAppBuilder))]

namespace VmMachineHwVersionUpdater.Avalonia.Tests;

public class TestApp : Application
{
    public override void Initialize()
    {
        Styles.Add(new FluentTheme());
    }
}