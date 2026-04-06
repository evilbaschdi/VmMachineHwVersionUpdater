---
applyTo: "**/Models/*.cs,**/Enums/*.cs"
---

# Models & Enums Instructions

When working with models and enums in this repository, follow these conventions.

## RawMachine (DTO)
`RawMachine` is a plain data transfer object populated during file parsing. All properties have default values:

```csharp
public class RawMachine
{
    // String properties default to ""
    string Annotation, DetailedData, DisplayName, EncryptionData,
           EncryptionEncryptedKey, EncryptionKeySafe, GuestOs,
           MksEnable3D, ManagedVmAutoAddVTpm, SyncTimeWithHost,
           ToolsUpgradePolicy, OSType = ""
    // Numeric properties
    int HwVersion, MemSize, CpuCount, VirtualBoxHwVersion
    MachineType MachineType
}
```

When adding properties to `RawMachine`:
- Always provide a default value (empty string for strings, 0 for numbers)
- This class is mutated during parallel parsing — keep properties simple

## Machine (UI-Bound Model)
`Machine` is the primary UI model implementing `INotifyPropertyChanged`:

### Constructor Dependencies
Machine has 5 injected services for modifying VMX files when properties change:
- `IToggleToolsSyncTime` — Updates `tools.syncTime`
- `IToggleToolsUpgradePolicy` — Updates `tools.upgrade.policy`
- `IToggleMksEnable3D` — Updates `mks.enable3d`
- `IUpdateMachineVersion` — Updates `virtualhw.version`
- `IUpdateMachineMemSize` — Updates `memsize`

### Property Categories

**Init-only reactive properties** (trigger VMX file writes via `init` setter):
- `AutoUpdateTools` (bool) → calls `IToggleToolsUpgradePolicy`
- `HwVersion` (int) → calls `IUpdateMachineVersion`
- `MemSize` (int, in GB) → calls `IUpdateMachineMemSize`
- `SyncTimeWithHost` (bool) → calls `IToggleToolsSyncTime`
- `Accelerate3DGraphics` (bool) → calls `IToggleMksEnable3D`

**Important**: These only write to disk when `IsEnabledForEditing == true` AND `PropertyChanged` is not null (i.e., UI is bound).

**Simple data properties** (no side effects):
- `DirectorySizeGb`, `MachineState`, `Annotation`, `Directory`, `DirectorySize`
- `DisplayName`, `EncryptionData`, `EncryptionEncryptedKey`, `EncryptionKeySafe`
- `ExtendedInformation`, `ExtendedInformationToolTip`, `GuestOs`, `GuestOsDetailedData`
- `LogLastDate`, `LogLastDateDiff`, `ManagedVmAutoAddVTpm`, `Path`, `ShortPath`
- `IsEnabledForEditing`, `MachineType`

### Creating Machine Instances
Machines are created in `HandleMachineFromPath` via constructor with DI services. Never instantiate `Machine` directly without proper dependency injection.

## MachinePath
Simple container for file discovery results:
```csharp
public class MachinePath
{
    string MachinePoolPath    // Parent pool directory
    string MachineFilePath    // Full .vmx/.vbox file path
}
```

## LoadHelper
Aggregation container returned by `ILoad`:
```csharp
public class LoadHelper
{
    ConcurrentDictionary<string, bool> SearchOsItems    // Unique OS types
    double? UpdateAllHwVersion                          // Max version for bulk update
    string UpdateAllTextBlocks                          // UI display text
    ConcurrentBag<Machine> VmDataGridItemsSource        // All loaded machines
}
```

## CurrentMachine
Writable singleton holding the currently selected `Machine`:
- Implements `ICurrentMachine : IWritableValue<Machine>`
- Get/set via `.Value` property
- Used by commands to know which machine the user selected

## Enums

### MachineState
```csharp
Off = 0,     // VM is powered off (normal state)
Paused = 1   // VM has .vmss file (suspended state, editing disabled)
```

### MachineType
```csharp
Vmx = 0,     // VMware Workstation/Player
Vbox = 1,    // VirtualBox
HyperV = 2   // Hyper-V (future support)
```

## Extension Methods (DoubleExtensions)
Uses C# preview `extension` property syntax:
```csharp
extension(double input)
{
    public double GiBiBytesToKiBiBytes()   // × 1,073,741,824
    public double KiBiBytesToGiBiBytes()   // ÷ 1,073,741,824
    public string ToFileSize(int precision, CultureInfo culture)
}
```
- Negative inputs throw `ArgumentOutOfRangeException`
- `ToFileSize` formats bytes to: B, KB, MB, GB, TB, PB, EB
