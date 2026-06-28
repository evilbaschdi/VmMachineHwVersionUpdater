using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EvilBaschdi.Core.Avalonia.DependencyInjection;
using VmMachineHwVersionUpdater.Avalonia.ViewModels;
using VmMachineHwVersionUpdater.Avalonia.Views;

namespace VmMachineHwVersionUpdater.Avalonia;

/// <inheritdoc />
public class App : ApplicationWithSplash
{
    /// <inheritdoc />
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    /// <inheritdoc />
    protected override bool ResizeWithBorder400 => true;

    /// <inheritdoc />
    protected override void PreMainWindowCreation()
    {
        ApplicationServices.AppName = Current?.Name;
    }

    /// <inheritdoc />
    protected override Window CreateMainWindow() => new MainWindow
                                                    {
                                                        DataContext = ApplicationServices.GetRequiredService<MainWindowViewModel>()
                                                    };
}