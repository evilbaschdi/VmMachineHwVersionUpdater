using Avalonia;

namespace VmMachineHwVersionUpdater.Avalonia.Tests;

public class TestAppBuilder
{
    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<TestApp>()
                                                             .UseHeadless(new AvaloniaHeadlessPlatformOptions());
}