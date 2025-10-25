---
applyTo: "**/*.cs"
---

# Code Style Instructions

When writing C# code in this repository, strictly follow these conventions:

## Required Language Features

- Use file-scoped namespaces (REQUIRED): `namespace Project.Module;`
- Use primary constructors with dependency injection when applicable
- Target frameworks: net9.0 and net10.0
- Language version: preview (cutting-edge C# features)

## Null Safety Requirements

All parameters MUST be validated for null:

```csharp
// For method parameters
public RawMachine ValueFor(string file)
{
    ArgumentNullException.ThrowIfNull(file);
    // Method implementation
}

// For constructor parameters with primary constructors
public class ParseVmxFile(
    [NotNull] IVmxLineStartsWith vmxLineStartsWith,
    [NotNull] ILineStartActions lineStartActions) : IParseVmxFile
{
    private readonly IVmxLineStartsWith _vmxLineStartsWith = 
        vmxLineStartsWith ?? throw new ArgumentNullException(nameof(vmxLineStartsWith));
}
```

## Interface Implementation Pattern

All implementations MUST inherit from corresponding interfaces:

```csharp
public class ParseVmxFile : IParseVmxFile
{
    /// <inheritdoc />
    public RawMachine ValueFor(string file)
    {
        // Implementation
    }
}
```

## Global Usings Available

These usings are available globally across all projects:
- EvilBaschdi.Core
- EvilBaschdi.Core.DependencyInjection
- EvilBaschdi.Core.Settings
- JetBrains.Annotations
- Microsoft.Extensions.DependencyInjection
- System.Linq
- VmMachineHwVersionUpdater.Core.BasicApplication
- VmMachineHwVersionUpdater.Core.DependencyInjection
- VmMachineHwVersionUpdater.Core.Models
- VmMachineHwVersionUpdater.Core.PerMachine
- VmMachineHwVersionUpdater.Core.Settings

## Naming Conventions

- Private fields: `_fieldName` (underscore prefix, camelCase)
- Public properties: `PropertyName` (PascalCase)
- Local variables: `variableName` (camelCase)
- Classes follow their interface: `IParseVmxFile` → `ParseVmxFile`