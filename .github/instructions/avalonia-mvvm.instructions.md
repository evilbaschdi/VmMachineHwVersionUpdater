---
applyTo: "**/ViewModels/**/*.cs,**/Views/**/*.cs,**/*.axaml,**/Views/**/*.axaml"
---

# Avalonia UI & MVVM Instructions

When working with Avalonia UI files in this repository, follow these conventions.

## MVVM Architecture

### View Resolution
Views are resolved automatically via `ViewLocator.cs`:
- ViewModel class name is transformed by replacing `ViewModel` → `View`
- Example: `MainWindowViewModel` → `MainWindowView` (but actual window is `MainWindow`)
- All ViewModels must inherit from `ViewModelBase` (which extends `ReactiveObject`)

### ViewModel Pattern
```csharp
namespace VmMachineHwVersionUpdater.Avalonia.ViewModels;

/// <inheritdoc cref="IMyViewModel" />
/// <inheritdoc cref="ViewModelBase" />
public class MyViewModel : ViewModelBase, IMyViewModel
{
    // Constructor with DI
    public MyViewModel([NotNull] IDependency dep)
    {
        _dep = dep ?? throw new ArgumentNullException(nameof(dep));
    }
}
```

### Reactive Commands Pattern
All UI commands follow this pattern:

**Interface** (in `ViewModels/Internal/`):
```csharp
namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc />
public interface IMyReactiveCommand : IReactiveCommandUnitTask;
```

**Implementation** (in `ViewModels/Internal/`):
```csharp
namespace VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal;

/// <inheritdoc cref="IMyReactiveCommand" />
/// <inheritdoc cref="ReactiveCommandUnitTask" />
public class MyReactiveCommand(
    [NotNull] IDependency dep,
    [NotNull] IMainWindowByApplicationLifetime mainWindowByApplicationLifetime)
    : ReactiveCommandUnitTask, IMyReactiveCommand
{
    private readonly IDependency _dep = dep ?? throw new ArgumentNullException(nameof(dep));
    private readonly IMainWindowByApplicationLifetime _mainWindowByApplicationLifetime =
        mainWindowByApplicationLifetime ?? throw new ArgumentNullException(nameof(mainWindowByApplicationLifetime));

    public override async Task RunAsync(CancellationToken cancellationToken = default)
    {
        // Implementation
    }
}
```

**Key imports for reactive commands:**
```csharp
using EvilBaschdi.Core.Avalonia;
using FluentAvalonia.UI.Controls;
```

### Command Aggregation
All 11 reactive commands are aggregated in `InitReactiveCommands`:
- Register new commands as properties in `IInitReactiveCommands` and `InitReactiveCommands`
- Wire commands in `MainWindowViewModel.Run()` method

## Dialog Patterns

### TaskDialog (for notifications/errors)
```csharp
var dialog = new TaskDialog
{
    Title = "Title",
    IconSource = new SymbolIconSource { Symbol = Symbol.AlertUrgentFilled },
    Buttons = { TaskDialogButton.OKButton },
    XamlRoot = mainWindow,
    Content = "Message"
};
await dialog.ShowAsync();
```

### ContentDialog (for confirmations)
```csharp
var dialog = new ContentDialog
{
    Title = "Confirm",
    Content = "Are you sure?",
    PrimaryButtonText = "Yes",
    CloseButtonText = "No"
};
var result = await dialog.ShowAsync(mainWindow);
```

## AXAML Conventions

### DataGrid Columns
- `DataGridTextColumn` — Read-only text display
- `DataGridCheckBoxColumn` — Boolean toggles (two-way binding)
- `DataGridTemplateColumn` — Custom templates (buttons, NumericUpDown)
- Grouping via `DataGridPathGroupDescription("Directory")`
- Sorting via `DataGridComparerSortDescription(MachineComparer, Ascending)`

### Theme & Styling
```xml
<FluentTheme />
<styling:FluentAvaloniaTheme PreferSystemTheme="True" PreferUserAccentColor="True" />
```
Uses FluentAvalonia for Windows 11 Fluent Design look.

### Binding Patterns
- Two-way: Search filters, selected machine, editable values
- One-way: Display text, enabled/readonly states, collections
- Commands: All use `ReactiveCommand<Unit, Unit>` from ReactiveUI

## Important: IMainWindowByApplicationLifetime
Most commands that show dialogs need `IMainWindowByApplicationLifetime` to access the main window as `XamlRoot`. Always inject it when creating dialog-showing commands.

## Avalonia Test Considerations
- Use `Avalonia.Headless` for UI testing
- Additional global using: `EvilBaschdi.Testing.Avalonia`
- Avalonia test project has conditional package references in `Directory.Build.props`
