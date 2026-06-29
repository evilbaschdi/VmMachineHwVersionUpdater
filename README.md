# VmMachineHwVersionUpdater

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg?style=for-the-badge)](LICENSE)

"Vm Machine Hardware Version Updater" is a small tool to manage VMware and VirtualBox machine properties such as changing the hardware version of multiple machines without editing the configuration files manually.

## ⚖️ Disclaimer

This project is a community-maintained open-source tool and is **not** affiliated with, endorsed by, sponsored by, or associated with Broadcom Inc., VMware Inc., Oracle Corporation, or any of their affiliates.

* **VMware**, **vSphere**, **ESXi**, and related terms are registered trademarks of Broadcom Inc.
* **VirtualBox** and related terms are registered trademarks of Oracle Corporation and/or its affiliates.

All product names, logos, and brands mentioned within this repository (including the description and documentation) are the property of their respective owners. They are used here solely for descriptive and informational purposes to indicate compatibility and interoperability.

## Features:

- Change hardware version of one or more machines
- Enable/disable "Synchronize guest time with host"
- Tools upgrade policy switch (upgradeAtPowerCycle/useGlobal)
- Multi-Pool aware by adding paths inside the settings section
- Archive functionality
- Add/edit annotations (VMware Workstation only)
- Start machine
- Open configuration file in VS Code
- Jump to path

![Screenshot](res/Screenshot.png)

[User & Contributing Guide](/docs/User-Guide.md) | [Solution Architecture](/docs/Solution-Architecture.md)

## Package Feeds

Default by NuGet.config is myget.org

|                                | Feed Url                                                         |
| :----------------------------- | :--------------------------------------------------------------- |
| ![myget.org][myGetBadge]       | <https://www.myget.org/F/evilbaschdi/api/v3/index.json>          |
| ![codeberg.org][codebergBadge] | <https://codeberg.org/api/packages/evilbaschdi/nuget/index.json> |

| main                                                         | develop                                                            |
| :----------------------------------------------------------- | :----------------------------------------------------------------- |
| [![CodeFactor][codeFactorMainBadge]][codeFactorMainOverview] | [![CodeFactor][codeFactorDevelopBadge]][codeFactorDevelopOverview] |


[codeFactorMainBadge]: https://www.codefactor.io/repository/github/evilbaschdi/VmMachineHwVersionUpdater/badge/main?style=for-the-badge
[codeFactorMainOverview]: https://www.codefactor.io/repository/github/evilbaschdi/VmMachineHwVersionUpdater/overview/main
[codeFactorDevelopBadge]: https://www.codefactor.io/repository/github/evilbaschdi/VmMachineHwVersionUpdater/badge/develop?style=for-the-badge
[codeFactorDevelopOverview]: https://www.codefactor.io/repository/github/evilbaschdi/VmMachineHwVersionUpdater/overview/develop
[myGetBadge]: https://img.shields.io/badge/MyGet.org-gray?style=for-the-badge&logo=myget
[codebergBadge]: https://img.shields.io/badge/Codeberg-gray?style=for-the-badge&logo=codeberg
