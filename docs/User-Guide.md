# User Guide for VmMachineHwVersionUpdater

Welcome to the **VmMachineHwVersionUpdater** user guide! This document will help you set up and use the application to manage your VMware and VirtualBox machine configurations efficiently.

## Table of Contents

- [User Guide for VmMachineHwVersionUpdater](#user-guide-for-vmmachinehwversionupdater)
  - [Table of Contents](#table-of-contents)
  - [Introduction](#introduction)
  - [Prerequisites](#prerequisites)
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
  - [Troubleshooting](#troubleshooting)
  - [Contribution](#contribution)

## Introduction

**VmMachineHwVersionUpdater** is a utility designed to inspect and modify configuration files for virtual machines (VMware `.vmx` and VirtualBox `.vbox`) without manually editing the text files. It allows you to:

- Bulk update hardware versions.
- Toggle "Synchronize guest time with host".
- Manage VM inventory from multiple locations ("Pools").
- Quickly access VM files or launch VMs.

## Prerequisites

- **Operating System**: Windows 10/11 (x64 or ARM64).
- **Runtime**: .NET 10.0 Runtime (if not running a self-contained version).
- **Virtualization Software**: VMware Workstation/Player or VirtualBox (to have VMs to manage).

## Installation

Currently, the application is distributed as a portable executable or built from source.

1.  **Download/Build**: Acquire the latest release or build the solution using Visual Studio 2026 or the .NET CLI (`dotnet build`).
2.  **Locate the Executable**:
    -   If built from source: Look in `VmMachineHwVersionUpdater.Avalonia/bin/Debug/net10.0/` (or `Release`).
    -   If downloaded: Extract the zip archive to a folder of your choice.

## Configuration

The application reads its configuration from JSON files located in the `Settings` subdirectory next to the executable.

### Setting up VM Pools

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

## Using the Application

### Overview

When you launch the application, it will scan the directories configured in `VmPools.json`. The main window displays a list of found virtual machines.

**Note**: The list supports **single selection only**. For bulk operations, see the features below.

### Features

#### Update Hardware Version
-   **Single VM**: Select a VM from the list and set the version directly in the "Version" column.
-   **Bulk Update**: To update all listed VMs at once, use the **Update All** section in the application footer. Enter the desired version in the numeric box and click the update button.
-   **Note**: These actions modify the configuration files directly. Ensure VMs are powered off before updating.

#### Sync Guest Time
Toggle the "Synchronize guest time with host" setting for selected VMs. This is useful for keeping VM clocks accurate without needing VMware Tools to be fully active or if you want to force specific behavior.

#### Archive VMs
Move a VM to one of the configured **Archive Paths**. This helps declutter your active VM pool without deleting the files.

#### Annotations
(VMware Workstation Only)
You can view and edit the "Annotation" (description) field of a VM directly from the tool.

#### Other Actions
-   **Start Machine**: Launches the VM using the associated application.
-   **Open .vmx in VS Code**: Quickly opens the configuration file for manual inspection.
-   **Go to Path**: Opens the folder containing the VM in File Explorer.

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

### Commands

To trigger a full release build, run the following from a PowerShell terminal in the root directory:

```powershell
./publish.ps1
```

**Note**: Since the application is published with the `--no-self-contained` flag, the target machine **must** have the .NET 10.0 Runtime installed to run the application.

## Troubleshooting

-   **No VMs found?**
    -   Check `Settings/VmPools.json` and ensure the paths are correct and accessible.
    -   Ensure the paths contain `.vmx` or `.vbox` files.
-   **Crash on startup?**
    -   Verify that the `Settings` folder exists and contains valid JSON files.
    -   Check if you have the required .NET Runtime installed.

## Contribution

Contributions are welcome! If you have ideas for new features or find bugs, feel free to:

1.  **Open an Issue**: Discuss your ideas or report problems.
2.  **Submit a Pull Request**:
    -   Fork the repository.
    -   Create a branch for your feature or fix.
    -   Ensure tests pass and the code follows the existing style.
    -   Open a PR against the `develop` branch.
