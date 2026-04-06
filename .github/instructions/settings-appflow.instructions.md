---
applyTo: "**/Settings/*.cs,**/Settings/*.json,**/BasicApplication/*.cs"
---

# Settings & Application Flow Instructions

When working with settings or application flow code, follow these conventions.

## Settings Architecture

### JSON Configuration Files
Located in `VmMachineHwVersionUpdater.Core/Settings/`:

**VmPools.json** — VM directory configuration:
```json
{
  "VmPool": [
    "C:\\vm\\VMware",
    "{UserProfile}/Documents/Virtual Machines"
  ],
  "ArchivePath": [
    "C:\\vm\\VMware\\_archive"
  ]
}
```

**GuestOsStringMapping.json** — OS ID to display name mapping (sourced from VMware open-vm-tools).

### Settings Classes Pattern
Settings files use `SettingsFromJsonFile` base class from EvilBaschdi.Core:

```csharp
// Interface inherits from ISettingsFromJsonFile
public interface IVmPools : ISettingsFromJsonFile;

// Implementation passes JSON file path to base
public class VmPools() : SettingsFromJsonFile("Settings/VmPools.json"), IVmPools;
```

### Path Resolution
`IReplaceUserProfilePlaceholder` replaces `{UserProfile}` tokens:
```csharp
// Resolves to Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)
// e.g., "{UserProfile}/vm" → "C:\Users\John\vm"
```

`IPathSettings` combines pool + archive paths with placeholder resolution:
```csharp
public interface IPathSettings
{
    List<string> ArchivePath { get; }
    List<string> VmPool { get; }
}
```

### Guest OS Mapping
`IGuestOsOutputStringMapping` resolves OS IDs to display names:
- Input: `"ubuntu64Guest"` → Output: `"Ubuntu 64-bit"`
- Falls back to raw input if no mapping found
- Uses `CachedValueFor<string, string>` for performance

## Application Data Flow

### Load Pipeline
```
ILoad.Value → IMachinesFromPath.Value → IPathSettings.VmPool
                                       → IFileListFromPath (scan for .vmx/.vbox)
                                       → IHandleMachineFromPath.ValueFor() (per file)
                                       → MachineParserStrategy.Parse()
                                       → Machine (fully populated)
```

### Load Result (LoadHelper)
`ILoad` returns a `LoadHelper` containing:
1. `VmDataGridItemsSource` — All `Machine` objects in a `ConcurrentBag<Machine>`
2. `SearchOsItems` — Unique guest OS types across all machines
3. `UpdateAllHwVersion` — Maximum HW version found (for bulk update default)
4. `UpdateAllTextBlocks` — UI text like "Update all 15 machines to version"

### Filtering Pipeline
```
User types search text → MainWindowViewModel.SearchFilterText setter
                        → IFilterDataGridCollectionView.RunFor((osText, filterText))
                        → IFilterItemSource.ValueFor((machine, osText, filterText))
                        → DataGridCollectionView.Filter = predicate
```

### Filter Logic (FilterItemSource)
1. **OS filter**: If searchOsText is set and not "(no filter)", machine.GuestOs must start with it
2. **Text filter**:
   - Empty → passes
   - Starts/ends with `*` → wildcard: all characters must exist in DisplayName OR Annotation
   - Otherwise → substring match against DisplayName OR Annotation

### Search OS Items Loading
`ILoadSearchOsItems` builds the OS dropdown:
1. Empty string (no filter)
2. Separator
3. Raw OS item keys from machines (sorted)
4. Separator
5. Friendly OS names from `IGuestOsesInUse` (sorted)

### ISeparator
Marker interface for dropdown separators. Implementation returns:
- Core: abstract `IObject` value
- Avalonia: concrete `new Separator()` control

## Build Configuration for Settings Files
`Directory.Build.targets` ensures JSON settings are copied:
```xml
<None Update="Settings\GuestOsStringMapping.json">
    <CopyToOutputDirectory>Always</CopyToOutputDirectory>
</None>
<None Update="Settings\VmPools.json">
    <CopyToOutputDirectory>Always</CopyToOutputDirectory>
</None>
```
