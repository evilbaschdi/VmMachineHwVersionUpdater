---
applyTo: "**/Commands/*.cs"
---

# Commands Instructions

When working with command classes, follow these conventions.

## Core Commands (VmMachineHwVersionUpdater.Core.Commands)

Core commands encapsulate user actions that operate on the current machine or application:

### Command Pattern
```csharp
namespace VmMachineHwVersionUpdater.Core.Commands;

/// <inheritdoc />
public class MyCommand(
    [NotNull] IProcessByPath processByPath,
    [NotNull] ICurrentMachine currentMachine) : IMyCommand
{
    private readonly IProcessByPath _processByPath =
        processByPath ?? throw new ArgumentNullException(nameof(processByPath));
    private readonly ICurrentMachine _currentMachine =
        currentMachine ?? throw new ArgumentNullException(nameof(currentMachine));

    /// <inheritdoc />
    public void Run()
    {
        // Implementation
    }
}
```

### Existing Core Commands

| Command | Interface | Depends On | Behavior |
|---------|-----------|------------|----------|
| `GoToCommand` | `IGoToCommand : IRun` | `IProcessByPath`, `ICurrentMachine` | Opens VM directory in file explorer |
| `StartCommand` | `IStartCommand : IRun` | `IProcessByPath`, `ICurrentMachine` | Launches VM file with associated app |
| `OpenWithCodeCommand` | `IOpenWithCodeCommand : ITaskRun` | `IProcessByPath`, `ICurrentMachine` | Opens VM file via `vscode://file/` protocol |
| `ReloadCommand` | `IReloadCommand : IRun` | `IProcessByPath` | Restarts the application |

### Adding a New Core Command
1. Create `INewCommand` interface inheriting `IRun` (sync) or `ITaskRun` (async)
2. Implement `NewCommand` with primary constructor pattern
3. Register in `ConfigureCommandServices.AddCommandServices()`
4. Create corresponding reactive command wrapper in Avalonia project if UI-triggered

## Reactive Commands (VmMachineHwVersionUpdater.Avalonia.ViewModels.Internal)

UI-layer command wrappers that add dialog behavior on top of core commands. See `avalonia-mvvm.instructions.md` for the reactive command pattern.

### Relationship: Core Command → Reactive Command
```
IStartCommand (Core)    → StartReactiveCommand (Avalonia) adds TaskDialog for no-selection error
IGoToCommand (Core)     → GoToReactiveCommand (Avalonia) wraps as async
IReloadCommand (Core)   → ReloadReactiveCommand (Avalonia) closes app window
```

Some reactive commands don't have core command counterparts — they operate directly on injected services:
- `DeleteReactiveCommand` uses `IDeleteMachine` + `ICurrentMachine`
- `CopyReactiveCommand` uses `ICopyMachine` + `ICopyProgress`
- `ArchiveReactiveCommand` uses `IArchiveMachine`
- `UpdateAllReactiveCommand` uses `IUpdateMachineVersion` + `ILoad`
- `RenameReactiveCommand` uses `IChangeDisplayName`
- `AddEditAnnotationReactiveCommand` shows `AddEditAnnotationDialog`
- `AboutWindowReactiveCommand` shows `AboutWindow`

## ICurrentMachine
All commands that operate on a specific machine access it via `ICurrentMachine.Value`. This is set by the DataGrid selection binding in `MainWindowViewModel.SelectedMachine`.
