# User Guide for VmMachineHwVersionUpdater

Welcome to the **VmMachineHwVersionUpdater** user guide! This document will help you set up and use the application to manage your VMware and VirtualBox machine configurations efficiently.

## Table of Contents

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
- [Troubleshooting](#troubleshooting)

## Introduction

**VmMachineHwVersionUpdater** is a utility designed to inspect and modify configuration files for virtual machines (VMware `.vmx` and VirtualBox `.vbox`) without manually editing the text files. It allows you to:

- Bulk update hardware versions.
- Toggle "Synchronize guest time with host".
- Manage VM inventory from multiple locations ("Pools").
- Quickly access VM files or launch VMs.

## Prerequisites

- **Operating System**: Windows 10/11 (x64 or ARM64).
- **Runtime**: .NET 9.0 Runtime (if not running a self-contained version).
- **Virtualization Software**: VMware Workstation/Player or VirtualBox (to have VMs to manage).

## Installation

Currently, the application is distributed as a portable executable or built from source.

1.  **Download/Build**: Acquire the latest release or build the solution using Visual Studio 2022 or the .NET CLI (`dotnet build`).
2.  **Locate the Executable**:
    -   If built from source: Look in `VmMachineHwVersionUpdater.Avalonia/bin/Debug/net9.0/` (or `Release`).
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
    "C:\vm\VMware",
    "D:\MyVirtualMachines",
    "{UserProfile}/Documents/Virtual Machines"
  ],
  "ArchivePath": [
    "C:\vm\VMware\_archive",
    "{UserProfile}/Documents/Virtual Machines/Archive"
  ]
}
```

-   **VmPool**: A list of directory paths where the application should search for virtual machines. It scans these directories recursively.
-   **ArchivePath**: A list of directory paths designated for archived VMs.
-   **{UserProfile}**: You can use this placeholder to refer to your user profile directory (e.g., `C:\Users\YourName`).

**Note**: Ensure the paths use double backslashes (``) on Windows or forward slashes (`/`).

## Using the Application

### Overview

When you launch the application, it will scan the directories configured in `VmPools.json`. The main window displays a list of found virtual machines with details like:
-   **Display Name**: The name of the VM.
-   **Version**: Current hardware version.
-   **State**: Whether it's On, Off, Suspended, etc. (Detected via lock files).
-   **Path**: Location on disk.

### Features

#### Update Hardware Version
1.  Select one or more VMs from the list (hold `Ctrl` or `Shift` for multiple selection).
2.  Use the context menu (right-click) or the toolbar buttons to change the hardware version.
3.  **Note**: This modifies the `.vmx` file directly. Ensure VMs are powered off before updating.

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

## Troubleshooting

-   **No VMs found?**
    -   Check `Settings/VmPools.json` and ensure the paths are correct and accessible.
    -   Ensure the paths contain `.vmx` or `.vbox` files.
-   **Crash on startup?**
    -   Verify that the `Settings` folder exists and contains valid JSON files.
    -   Check if you have the required .NET Runtime installed.
