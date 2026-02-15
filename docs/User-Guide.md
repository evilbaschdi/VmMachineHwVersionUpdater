# User and Contributing Guide for VmMachineHwVersionUpdater

Welcome to the comprehensive guide for **VmMachineHwVersionUpdater**! This document covers everything from basic usage for end-users to detailed technical instructions for developers and contributors.

## Table of Contents

- [User and Contributing Guide for VmMachineHwVersionUpdater](#user-and-contributing-guide-for-vmmachinehwversionupdater)
  - [Table of Contents](#table-of-contents)
  - [Introduction](#introduction)
  - [User Guide](#user-guide)
    - [Prerequisites (User)](#prerequisites-user)
    - [Installation](#installation)
    - [Configuration](#configuration)
      - [Setting up VM Pools](#setting-up-vm-pools)
    - [Using the Application](#using-the-application)
      - [Overview](#overview)
      - [Features](#features)
        - [Update Hardware Version](#update-hardware-version)
        - [Sync Guest Time](#sync-guest-time)
        - [Archive VMs](#archive-vms)
        - [Annotations](#annotations)
        - [Other Actions](#other-actions)
  - [Building and Publishing](#building-and-publishing)
    - [Build Process](#build-process)
  - [Troubleshooting](#troubleshooting)
  - [Contributing Guide](#contributing-guide)
    - [Developer Prerequisites](#developer-prerequisites)
      - [Required Software](#required-software)
      - [Recommended Tools](#recommended-tools)
    - [Getting Started](#getting-started)
      - [1. Fork and Clone the Repository](#1-fork-and-clone-the-repository)
      - [2. Restore Dependencies](#2-restore-dependencies)
      - [3. Build the Project](#3-build-the-project)
      - [4. Run the Application](#4-run-the-application)
      - [5. Run Tests](#5-run-tests)
    - [Understanding the Project Structure](#understanding-the-project-structure)
      - [VmMachineHwVersionUpdater.Core](#vmmachinehwversionupdatercore)
      - [VmMachineHwVersionUpdater.Avalonia](#vmmachinehwversionupdateravalonia)
      - [Test Projects](#test-projects)
    - [Code Style and Conventions](#code-style-and-conventions)
      - [1. File-Scoped Namespaces (REQUIRED)](#1-file-scoped-namespaces-required)
      - [2. Primary Constructors with Dependency Injection](#2-primary-constructors-with-dependency-injection)
      - [3. Null Safety Required](#3-null-safety-required)
    - [Testing Requirements](#testing-requirements)
      - [Mandatory Test Structure](#mandatory-test-structure)
      - [Coverage Targets](#coverage-targets)
    - [Contributing Workflow](#contributing-workflow)
    - [Reporting Issues and Requesting Features](#reporting-issues-and-requesting-features)
  - [License and Copyright](#license-and-copyright)
    - [Copyright Notice](#copyright-notice)
    - [Important Licensing Information](#important-licensing-information)

---

## Introduction

**VmMachineHwVersionUpdater** is a utility designed to inspect and modify configuration files for virtual machines (VMware `.vmx` and VirtualBox `.vbox`) without manually editing the text files. It allows you to:

- Bulk update hardware versions.
- Toggle "Synchronize guest time with host".
- Manage VM inventory from multiple locations ("Pools").
- Quickly access VM files or launch VMs.

---

## User Guide

### Prerequisites (User)

- **Operating System**: Windows 10/11 (x64 or ARM64).
- **Runtime**: .NET 10.0 Runtime (if not running a self-contained version).
- **Virtualization Software**: VMware Workstation/Player or VirtualBox (to have VMs to manage).

### Installation

Currently, the application is distributed as a portable executable or built from source.

To trigger a full release build, run the following from a PowerShell terminal in the root directory:

```powershell
./publish.ps1
```

**Note**: Since the application is published with the `--no-self-contained` flag, the target machine **must** have the .NET 10.0 Runtime installed to run the application.

### Configuration

The application reads its configuration from JSON files located in the `Settings` subdirectory next to the executable.

#### Setting up VM Pools

To tell the application where your Virtual Machines are located, you must edit the `Settings/VmPools.json` file.

1.  Navigate to the folder containing `VmMachineHwVersionUpdater.Avalonia.exe`.
2.  Open the `Settings` folder.
3.  Open `VmPools.json` with a text editor (Notepad, VS Code, etc.).

**Structure:**

```json
{
  "VmPool": [
    "C:\\vm\\VMware",
    "D:\\MyVirtualMachines",
    "{UserProfile}/Documents/Virtual Machines"
  ],
  "ArchivePath": [
    "C:\\vm\\VMware\\_archive",
    "{UserProfile}/Documents/Virtual Machines/Archive"
  ]
}
```

-   **VmPool**: A list of directory paths where the application should search for virtual machines. It scans these directories recursively.
-   **ArchivePath**: A list of directory paths designated for archived VMs.
-   **{UserProfile}**: You can use this placeholder to refer to your user profile directory (e.g., `C:\Users\YourName`).

**Note**: Ensure the paths use double backslashes (`\\`) on Windows or forward slashes (`/`).

### Using the Application

#### Overview

When you launch the application, it will scan the directories configured in `VmPools.json`. The main window displays a list of found virtual machines.

**Note**: The list supports **single selection only**. For bulk operations, see the features below.

#### Features

##### Update Hardware Version
-   **Single VM**: Select a VM from the list and set the version directly in the "Version" column.
-   **Bulk Update**: To update all listed VMs at once, use the **Update All** section in the application footer. Enter the desired version in the numeric box and click the update button.
-   **Note**: These actions modify the configuration files directly. Ensure VMs are powered off before updating.

##### Sync Guest Time
Toggle the "Synchronize guest time with host" setting for selected VMs. This is useful for keeping VM clocks accurate without needing VMware Tools to be fully active or if you want to force specific behavior.

##### Archive VMs
Move a VM to one of the configured **Archive Paths**. This helps declutter your active VM pool without deleting the files.

##### Annotations
(VMware Workstation Only)
You can view and edit the "Annotation" (description) field of a VM directly from the tool.

##### Other Actions
-   **Start Machine**: Launches the VM using the associated application.
-   **Open .vmx in VS Code**: Quickly opens the configuration file for manual inspection.
-   **Go to Path**: Opens the folder containing the VM in File Explorer.

---

## Building and Publishing

For developers or system administrators who want to build the application from source, a `publish.ps1` script is provided in the root directory.

### Build Process

1.  **Clean and Restore**: The script first cleans the solution, restores NuGet packages, and performs a standard build.
2.  **Publishing**: It then triggers the project-specific publish script in the Avalonia project.
3.  **Target Runtimes**: The application is published for:
    -   `win-x64` (Standard 64-bit Windows)
    -   `win-arm64` (Windows on ARM)
4.  **Framework**: It targets `.net10.0`.
5.  **Output Location**: By default, the published files are placed in `C:\Apps\VmMachineHwVersionUpdater.Avalonia`.
6.  **Launcher**: The script attempts to copy a generic `AppLauncher.exe` from `C:\Apps\AppLauncher` to the output directory and renames it to match the application name.

---

## Troubleshooting

-   **No VMs found?**
    -   Check `Settings/VmPools.json` and ensure the paths are correct and accessible.
    -   Ensure the paths contain `.vmx` or `.vbox` files.
-   **Crash on startup?**
    -   Verify that the `Settings` folder exists and contains valid JSON files.
    -   Check if you have the required .NET Runtime installed.

---

## Contributing Guide

Thank you for your interest in contributing to VmMachineHwVersionUpdater! We welcome contributions from the community and appreciate your time and effort in helping improve this project.

### Developer Prerequisites

Before you begin contributing, ensure you have the following software installed on your development machine.

#### Required Software

- **.NET SDK 10.0.103** - This version is required as specified in the project's `global.json` file.
- **IDE** - Choose one of the following:
  - **Visual Studio 2026** (Required for .NET 10 support)
  - **JetBrains Rider**
  - **Visual Studio Code** with C# Dev Kit extension
- **Git** - For version control and cloning the repository

#### Recommended Tools

- **PowerShell** - Required for running the project's publish scripts.
- **Windows OS** - The application currently targets Windows (win-x64 and win-arm64 runtimes) but is also able to run under Linux (tested with Debian and Ubuntu).

### Getting Started

#### 1. Fork and Clone the Repository

First, fork the repository on GitHub, then clone your fork:

```bash
git clone https://github.com/YOUR-USERNAME/VmMachineHwVersionUpdater.git
cd VmMachineHwVersionUpdater
```

#### 2. Restore Dependencies

The project uses `Directory.Build.props` for centralized configuration and centralized package management. Restore all NuGet packages:

```bash
dotnet restore
```

#### 3. Build the Project

Build the entire solution to verify everything is set up correctly:

```bash
dotnet build
```

The build must complete without any warnings or errors. All code must compile cleanly as part of the validation pipeline.

#### 4. Run the Application

To run the Avalonia UI application:

```bash
cd VmMachineHwVersionUpdater.Avalonia
dotnet run
```

#### 5. Run Tests

Verify that all tests pass before making any changes:

```bash
# Navigate back to the solution root
cd ..

# Run all tests
dotnet test
```

### Understanding the Project Structure

The solution contains 4 main projects organized into core logic, UI, and test projects.

#### VmMachineHwVersionUpdater.Core
Contains all business logic, parsing, and operations.
- **`BasicApplication/`**: VM discovery and loading logic.
- **`Commands/`**: User action implementations.
- **`PerMachine/`**: VM file parsing and operations (VMX/VBOX).
- **`Models/`**: Data models.
- **`Settings/`**: Configuration management.

#### VmMachineHwVersionUpdater.Avalonia
Provides the cross-platform desktop UI using the MVVM pattern.
- **`Views/`**: XAML UI views.
- **`ViewModels/`**: MVVM view models.

#### Test Projects
- **`VmMachineHwVersionUpdater.Core.Tests`**: Unit tests for core logic.
- **`VmMachineHwVersionUpdater.Avalonia.Tests`**: UI tests with Avalonia.Headless.

### Code Style and Conventions

This project follows strict code style conventions enforced at build time. Violations will cause build failures.

#### 1. File-Scoped Namespaces (REQUIRED)
All C# files MUST use file-scoped namespace declarations.

#### 2. Primary Constructors with Dependency Injection
Use primary constructors with `[NotNull]` attributes and null-check assignments.

#### 3. Null Safety Required
All parameters must be validated with `ArgumentNullException.ThrowIfNull()`.

### Testing Requirements

#### Mandatory Test Structure
Every test class MUST include three foundational tests:
1.  **Constructor_HasNullGuards**: Validates constructor parameter protection.
2.  **Constructor_ReturnsInterfaceName**: Verifies interface implementation.
3.  **Methods_HaveNullGuards**: Tests public method parameter validation.

#### Coverage Targets
All production code must maintain **90%+ line coverage** and **80%+ branch coverage**.

### Contributing Workflow

1.  **Create a Feature Branch**: Branch from `develop`.
2.  **Make Your Changes**: Follow code style and maintain test coverage.
3.  **Commit**: Use descriptive messages in the imperative mood.
4.  **Push and PR**: Push to your fork and open a PR against the `develop` branch of the main repository.

### Reporting Issues and Requesting Features

Please use GitHub Issues to report bugs or request features. Include clear descriptions, steps to reproduce, and environment details.

---

## License and Copyright

### Copyright Notice
Copyright © 2014 - 2026 evilbaschdi (Sebastian Walter)

### Licensing Information
This project is licensed under the **MIT License**. See the [LICENSE](LICENSE) file in the root of the repository for the full license text.

The MIT License is a permissive license that allows for free use, modification, and distribution of the software, provided that the copyright notice and permission notice are preserved.

---

Thank you for contributing to VmMachineHwVersionUpdater! 🚀