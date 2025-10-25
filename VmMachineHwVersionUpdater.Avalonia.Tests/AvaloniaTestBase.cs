namespace VmMachineHwVersionUpdater.Avalonia.Tests;

/// <summary>
/// Base class for Avalonia tests that ensures the headless platform is initialized
/// </summary>
public abstract class AvaloniaTestBase
{
    protected AvaloniaTestBase()
    {
        TestAppBuilder.EnsureInitialized();
    }
}