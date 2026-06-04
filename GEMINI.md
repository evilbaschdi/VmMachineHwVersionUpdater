# VmMachineHwVersionUpdater Project Mandates

This file serves as the foundational instruction set for Gemini CLI. It references and prioritizes the specialized instructions found in the `.github/instructions/` directory.

## Core Directives

- **One Type Per File**: Every interface and class MUST be stored in its own separate file. Never combine interfaces and implementations in a single `.cs` file.
- **Null Safety**: All parameters must be validated for null using `ArgumentNullException.ThrowIfNull` or primary constructor null-coalescing checks.
- **File-Scoped Namespaces**: Always use `namespace Project.Module;` syntax.
- **Testing**: Every change must be verified with unit tests following the established patterns in `testing.instructions.md`.

## Detailed Instructions

The following specialized instruction sets take absolute precedence over general defaults:

- [Avalonia UI & MVVM](.github/instructions/avalonia-mvvm.instructions.md)
- [Build System & Configuration](.github/instructions/build-system.instructions.md)
- [C# Code Style](.github/instructions/code-style.instructions.md)
- [Command Patterns](.github/instructions/commands.instructions.md)
- [Dependency Injection](.github/instructions/dependency-injection.instructions.md)
- [Project Structure](.github/instructions/dotnet-project-structure.instructions.md)
- [Models & Enums](.github/instructions/models-enums.instructions.md)
- [Settings & App Flow](.github/instructions/settings-appflow.instructions.md)
- [Unit Testing Standards](.github/instructions/testing.instructions.md)
- [VM Parsing Logic](.github/instructions/vm-parsing.instructions.md)

## Development Workflow

1. **Research**: Map the codebase and validate assumptions.
2. **Strategy**: Formulate a plan before execution.
3. **Execution**: Apply surgical changes and include automated tests.
4. **Validation**: Run project-specific build and test commands to confirm success.
