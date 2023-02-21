using Avalonia;
using Avalonia.ReactiveUI;

// ReSharper disable once ClassNeverInstantiated.Global
namespace VmMachineHwVersionUpdater.Avalonia;

internal class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    // ReSharper disable once MemberCanBePrivate.Global
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
                     .UsePlatformDetect()
                     .LogToTrace()
                     .UseReactiveUI()
                     .With(new Win32PlatformOptions
                           {
                               UseWindowsUIComposition = false, // it's enabled by default, but breaks rounded corners since v11 
                               CompositionBackdropCornerRadius = 8f
                           });
}