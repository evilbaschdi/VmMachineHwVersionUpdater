---
applyTo: "**/*.csproj,**/Directory.Build.props,**/Directory.Build.targets,**/Directory.Packages.props,**/global.json,**/*.slnx"
---

# Build System & Project Configuration Instructions

When working with project files, build props, or solution configuration, follow these conventions.

## SDK & Framework

- **.NET SDK**: Version pinned in `global.json` (currently `10.0.201`)
- **Target Framework**: `net10.0` (set in `Directory.Build.props`)
- **Language Version**: `preview` (enables cutting-edge C# features like extension members)
- **Implicit Usings**: Enabled globally

## Centralized Configuration

### Directory.Build.props (Root)
Central configuration for ALL projects:

**Shared settings:**
- Authors, Company, Copyright, Description, Product
- TargetFramework, LangVersion, ImplicitUsings
- Version: Dynamic `$([System.DateTime]::UtcNow.ToString(yyyy.M.d.H))`
- DocumentationFile, EnableNETAnalyzers, PublishSingleFile

**Test project auto-detection** (projects ending with `.Tests`):
- Sets `IsTestProject=true`, `OutputType=Exe`
- Disables documentation generation
- Enables `TestingPlatformDotnetTestSupport`

**Global usings** (all projects):
```
EvilBaschdi.Core, EvilBaschdi.Core.AppHelpers, EvilBaschdi.Core.DependencyInjection,
EvilBaschdi.Core.Settings, JetBrains.Annotations, Microsoft.Extensions.DependencyInjection,
System.Linq, and all VmMachineHwVersionUpdater.Core.* namespaces
```

**Test-only global usings:**
```
AutoFixture.Idioms, AutoFixture.Xunit3, EvilBaschdi.Testing,
FluentAssertions, FluentAssertions.Microsoft.Extensions.DependencyInjection,
NSubstitute, NSubstitute.ReturnsExtensions, Xunit
```

**Avalonia.Tests-only global usings:**
```
Avalonia.Controls, Avalonia.Headless, EvilBaschdi.Testing.Avalonia
```

**Shared package references** (all projects):
- `EvilBaschdi.Core`, `EvilBaschdi.Core.DependencyInjection`, `EvilBaschdi.Core.Settings`

**Test-only packages** (auto-added for `.Tests` projects):
- `EvilBaschdi.Testing`, `xunit.v3`, `xunit.v3.runner.utility`, `Microsoft.NET.Test.Sdk`
- `coverlet.collector`, `FluentAssertions.Analyzers`, `Meziantou.FluentAssertionsAnalyzers`
- `NSubstitute.Analyzers.CSharp`, `xunit.analyzers`, `xunit.runner.visualstudio`

### Directory.Build.targets (Root)
- Excludes `TestResults\**` from compilation
- Core projects: JSON settings files marked as `CopyToOutputDirectory=Always`
- Non-Core/non-Test projects: auto-reference Core project
- Test projects: auto-reference their corresponding production project

### Directory.Packages.props (Root)
Central Package Version Management (`ManagePackageVersionsCentrally=true`):
- All NuGet package versions defined centrally
- Individual `.csproj` files reference packages WITHOUT version numbers
- `CentralPackageTransitivePinningEnabled=false`

## Individual .csproj Files

### Convention: Minimal .csproj
Project files should be as minimal as possible. Most config is inherited:

**Core project** (`VmMachineHwVersionUpdater.Core.csproj`):
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <!-- Inherits everything from Directory.Build.props -->
</Project>
```

**Avalonia project** adds UI-specific settings:
```xml
<OutputType>WinExe</OutputType>
<ApplicationIcon>Assets\b.ico</ApplicationIcon>
<BuiltInComInteropSupport>true</BuiltInComInteropSupport>
```

**Test projects** add no extra configuration (auto-detected by name).

## Adding a New Package
1. Add version in `Directory.Packages.props`: `<PackageVersion Include="PackageName" Version="x.y.z" />`
2. Reference in `.csproj` without version: `<PackageReference Include="PackageName" />`
3. If shared across all projects, add in `Directory.Build.props` ItemGroup
4. If test-only, add in the test-conditional ItemGroup

## EditorConfig Enforcement
`.editorconfig` enforces:
- `csharp_style_namespace_declarations = file_scoped:error` — Build breaks on block-scoped namespaces
- CA1859, CA1860 suppressed in production code
- Test projects have ~60 CA rules set to error level
- `NS1001.severity = silent` — NSubstitute analyzer adjustment
