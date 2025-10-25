using Avalonia;

namespace VmMachineHwVersionUpdater.Avalonia.Tests;

public static class TestAppBuilder
{
    private static bool _isInitialized;
    private static readonly Lock Lock = new Lock();

    public static void EnsureInitialized()
    {
        lock (Lock)
        {
            if (_isInitialized)
            {
                return;
            }
        }

        lock (Lock)
        {
            if (_isInitialized)
            {
                return;
            }

            BuildAvaloniaApp().SetupWithoutStarting();
            _isInitialized = true;
        }
    }

    private static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
                     .UseHeadless(new AvaloniaHeadlessPlatformOptions
                                  {
                                      UseHeadlessDrawing = true
                                  });
}