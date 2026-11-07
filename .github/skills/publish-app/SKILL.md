---
name: publish-app
description: 'Publish this application by running scripts/publish.ps1. Use when the user says "run publish", "publish app", "publish the app", or asks to create the configured runtime builds.'
---

# Publish the Application

Run the repository's existing publish script when the user explicitly requests a publish, for example with "run publish".

## Procedure

1. Confirm the current workspace is the VmMachineHwVersionUpdater repository.
2. From the repository root, run `.\scripts\publish.ps1` in PowerShell and wait for it to finish.
3. Review the output for build and publish failures; do not report success based only on the process exit code.
4. Confirm the configured runtime output folders under `C:\Apps\VmMachineHwVersionUpdater.Avalonia` were updated.
5. Report the result and the runtime output locations. If any build, publish, or verification step failed, state the failure clearly.

The script cleans, restores, and builds the solution, then publishes the configured `win-x64` and `win-arm64` profiles. It writes outputs under `C:\Apps`, removes PDB files from the publish folders, signs binaries if a matching certificate is available, and copies the AppLauncher executable to the app's output directory. These steps can overwrite existing files at those locations.
