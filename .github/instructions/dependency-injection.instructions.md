---
applyTo: "**/DependencyInjection/*.cs"
---

# Dependency Injection Instructions

When working with DI registration files in this repository, follow these conventions strictly.

## Registration Pattern

All DI configuration is done via static extension methods on `IServiceCollection`:

```csharp
namespace VmMachineHwVersionUpdater.Core.DependencyInjection;

/// <summary />
public static class ConfigureXxxServices
{
    /// <summary />
    public static void AddXxxServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddSingleton<IService, ServiceImpl>();
    }
}
```

## Registration Rules

1. **Validate `services` parameter**: Always call `ArgumentNullException.ThrowIfNull(services)` first
2. **Singleton by default**: Almost all services are registered as `AddSingleton<TInterface, TImpl>()`
3. **Transient exceptions**: Use `AddTransient` only for UI controls that must be freshly created (e.g., `AddEditAnnotationDialog`, `ISeparator` → `AvaloniaControlsSeparator`)
4. **Alphabetical ordering**: Service registrations MUST be in alphabetical order by interface name
5. **No scoped services**: This is a desktop app — scoped lifetime is not used

## DI Module Organization

The DI is split across two projects:

### Core (`VmMachineHwVersionUpdater.Core.DependencyInjection`)
- `ConfigureCoreServices.AddCoreServices()` — All domain services (~35 singletons)
- `ConfigureCommandServices.AddCommandServices()` — 4 command services (GoTo, OpenWithCode, Reload, Start)

### Avalonia (`VmMachineHwVersionUpdater.Avalonia.DependencyInjection`)
- `ConfigureAvaloniaServices.AddAvaloniaServices()` — UI-specific services (Separator, Comparer, DataGrid config, etc.)
- `ConfigureReactiveCommandServices.AddReactiveCommandServices()` — 11 reactive command services + `IInitReactiveCommands`
- `ConfigureWindowsAndViewModels.AddWindowsAndViewModels()` — ViewModels (singleton) and dialogs (transient)

### Wiring in Program.cs
```csharp
serviceCollection.AddCoreServices();
serviceCollection.AddAboutServices();       // From EvilBaschdi.About.Avalonia
serviceCollection.AddCommandServices();
serviceCollection.AddAvaloniaServices();
serviceCollection.AddReactiveCommandServices();
serviceCollection.AddWindowsAndViewModels();
```

## Adding a New Service

When adding a new service:
1. Create the interface `INewService` and implementation `NewService`
2. Register in the appropriate `Configure*Services` class (Core for domain logic, Avalonia for UI)
3. Insert the registration line in alphabetical order
4. Add a corresponding test in the DI test class verifying the registration

## External Dependencies from EvilBaschdi.Core

These are registered in `ConfigureCoreServices`:
- `ICopyDirectoryWithFilesWithProgress`, `ICopyDirectoryWithProgress`, `ICopyProgress` — File copy operations
- `IFileListFromPath` — File discovery
- `IProcessByPath` — Process launching

Import them from: `using EvilBaschdi.Core.Internal;` and `using EvilBaschdi.Core.Internal.Copy;`
