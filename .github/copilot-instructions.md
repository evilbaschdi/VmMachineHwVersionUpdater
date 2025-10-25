# VmMachineHwVersionUpdater Repository Instructions

## Project Overview

This is a .NET solution that inspects and manages VM configurations for VMware Workstation/Player (.vmx) and VirtualBox (.vbox) files. It provides a cross-platform desktop UI built with Avalonia and a core library for parsing, listing, and updating VM metadata.

## Project Structure

- **VmMachineHwVersionUpdater.Core**: Domain models, parsing logic, commands, settings, dependency injection
- **VmMachineHwVersionUpdater.Avalonia**: Desktop UI with MVVM pattern, reactive commands, views
- **Tests**: Comprehensive unit tests for both Core and Avalonia layers using xUnit v3

## Build & Run Instructions

Always run commands from the repository root directory.

### Prerequisites
- .NET 9.0 or 10.0 SDK
- Windows, macOS, or Linux

### Build Steps
```bash
# Clean and restore
dotnet clean
dotnet restore
dotnet build

# Run tests
dotnet test

# Publish for specific runtime
dotnet publish VmMachineHwVersionUpdater.Avalonia/VmMachineHwVersionUpdater.Avalonia.csproj -c Release -r win-x64 --no-self-contained
```

### Key Architecture Components

- **Parsing Strategy**: `MachineParserStrategy` routes .vmx/.vbox files to appropriate parsers
- **VMX Parsing**: `ParseVmxFile` with `LineStartActions` for parallel line processing
- **VBox Parsing**: `ParseVboxFile` using XDocument for XML parsing
- **Discovery**: `MachinesFromPath` scans configured VM pools and archives
- **Models**: `RawMachine` (parsed data) → `Machine` (UI-bound with behavior)
- **Commands**: Reactive commands for Start, Reload, GoTo, OpenWithCode operations

## Critical Development Rules

### File-Scoped Namespaces (Build Error if Violated)
ALL C# files must use file-scoped namespaces:
```csharp
namespace VmMachineHwVersionUpdater.Core.PerMachine;

public class ParseVmxFile : IParseVmxFile
{
    // Class implementation
}
```

### Mandatory Test Structure
Every test class requires these three tests:
1. `Constructor_HasNullGuards` - Validates constructor null protection
2. `Constructor_ReturnsInterfaceName` - Verifies interface implementation  
3. `Methods_HaveNullGuards` - Tests method null safety

Use `[Theory, NSubstituteOmitAutoPropertiesTrueAutoData]` and FluentAssertions for all tests.

### Dependency Injection Pattern
All classes follow constructor injection with null guards:
```csharp
public class Service([NotNull] IDependency dependency) : IService
{
    private readonly IDependency _dependency = 
        dependency ?? throw new ArgumentNullException(nameof(dependency));
}
```

## Configuration Files

- **VmPools.json**: VM discovery paths with `{UserProfile}` placeholder support
- **GuestOsStringMapping.json**: OS string to display name mapping
- **Directory.Build.props**: Global project settings, usings, and test framework configuration
- **.editorconfig**: Enforces file-scoped namespaces and diagnostic rules

## Performance Considerations

- Use `Parallel.ForEach` for file processing in `MachinesFromPath` and `ParseVmxFile`
- Use `ConcurrentBag<T>` for thread-safe collection operations
- VMX parsing uses action lookup dictionary for O(1) line prefix matching

## Validation Pipeline

All changes must pass:
1. Compilation without warnings
2. All unit tests (xUnit v3 with comprehensive coverage)
3. Code analysis rules (CA1000-CA2243 set to error level in tests)
4. EditorConfig compliance (file-scoped namespaces enforced)

Trust these instructions and only search for additional information if the provided details are incomplete or found to be incorrect.