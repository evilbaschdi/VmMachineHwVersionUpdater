---
applyTo: "**/PerMachine/*.cs,**/Strategies/*.cs"
---

# VM Parsing & File Operations Instructions

When working with VM parsing or file operation code in this repository, follow these conventions.

## Parser Architecture

### Strategy Pattern
`MachineParserStrategy` dispatches to the correct parser based on file extension:
- `.vmx` → `ParseVmxFile` (VMware Workstation/Player)
- `.vbox` → `ParseVboxFile` (VirtualBox)
- Unknown extensions throw `NotSupportedException`

### Adding a New VM Format
1. Create `IParseNewFormat : IValueFor<string, RawMachine>` in `PerMachine/`
2. Implement `ParseNewFormat` with the parsing logic
3. Add a new case in `MachineParserStrategy.Parse()` switch expression
4. Add `MachineType` enum value in `Enums/MachineType.cs`
5. Register in `ConfigureCoreServices.AddCoreServices()`
6. Update `HandleMachineFromPath` if the new format needs special handling

## VMX Parsing Pipeline

### Line-by-Line Processing
VMX files are key-value text files: `key = "value"`

1. `ParseVmxFile.ValueFor(file)` reads all lines
2. Each line is checked against `IVmxLineStartsWith` (case-insensitive `StartsWith`)
3. Matching lines trigger actions from `ILineStartActions` dictionary
4. Actions extract values via `IReturnValueFromVmxLine` and populate `RawMachine`
5. Processing is parallelized with `Parallel.ForEach`

### Adding a New VMX Key
1. Add a new entry in `LineStartActions` constructor's dictionary:
```csharp
{ "newkey", (rawMachine, line) => rawMachine.NewProperty = _returnValueFromVmxLine.ValueFor(line, "newkey") }
```
2. Add the corresponding property to `RawMachine`
3. Map it in `HandleMachineFromPath.ValueFor()` to the `Machine` model

### VMX Value Extraction
`ReturnValueFromVmxLine.ValueFor(line, key)` strips the key prefix, `=`, and quotes:
- Input: `guestos = "ubuntu64Guest"` → Output: `ubuntu64Guest`

### Annotation Line Breaks
VMX stores line breaks as `|0D` (CR) and `|0A` (LF). `ConvertAnnotationLineBreaks` handles conversion.

## VMX File Modification (UpsertVmxLine<T>)

### Base Class Pattern
All VMX file writers inherit from `UpsertVmxLine<T>`:

```csharp
// For string values:
public class ChangeDisplayName() : UpsertVmxLine<string>("displayName")

// For bool values with true/false mappings:
public class ToggleToolsSyncTime() : UpsertVmxLine<bool>("tools.syncTime", "TRUE", "FALSE")

// For int values:
public class UpdateMachineVersion() : UpsertVmxLine<int>("virtualhw.version")
```

### UpsertVmxLine Behavior
- Reads all lines, finds matching key (case-insensitive)
- If found: replaces the line with new value
- If not found: appends new line at end
- Writes entire file back
- Implements `IDisposable`
- Rejects values containing `"` (double quote) for security

### Existing VMX Property Updaters
| Class | Key | Type | Values |
|-------|-----|------|--------|
| `UpdateMachineVersion` | `virtualhw.version` | `int` | Version number |
| `UpdateMachineMemSize` | `memsize` | `int` | Memory in MB |
| `ToggleToolsSyncTime` | `tools.syncTime` | `bool` | TRUE/FALSE |
| `ToggleToolsUpgradePolicy` | `tools.upgrade.policy` | `bool` | upgradeAtPowerCycle/useGlobal |
| `ToggleMksEnable3D` | `mks.enable3d` | `bool` | TRUE/FALSE |
| `AddEditAnnotation` | `annotation` | `string` | Free text |
| `ChangeDisplayName` | `displayName` | `string` | VM name |

## VBox Parsing
`ParseVboxFile` uses `System.Xml.Linq` with namespace `http://www.virtualbox.org/`:
- Reads `<Machine>` attributes: `name`, `version`
- Reads `<Hardware>` children: `<Memory>`, `<CPU>`, `<Description>`
- Maps `OSType` attribute for guest OS identification
- VBox machines have limited editing support (read-only in many cases)

## Machine Lifecycle Operations

### HandleMachineFromPath
The central orchestrator that converts a file path to a fully populated `Machine` object:
1. Parses file via `MachineParserStrategy`
2. Resolves guest OS display name
3. Reads log information (VMware only)
4. Calculates directory size
5. Determines edit capability (disabled for paused/encrypted VMs)
6. Sets extended information icons (📄 annotation, 🔐 TPM, 🕶 read-only)

### File Operations
- `ArchiveMachine`: Moves VM directory to archive path
- `CopyMachine`: Copies VM directory with progress callback
- `DeleteMachine`: Recursively deletes VM directory

## Thread Safety
- `MachinesFromPath` uses `ConcurrentBag<Machine>` for parallel collection
- `ParseVmxFile` uses `Parallel.ForEach` for line processing
- `UpdateMachineVersion.RunFor(ParallelQuery)` updates machines in parallel
- Always use thread-safe collections when aggregating results from parallel operations

## Memory Unit Conversions
- VMX stores `memsize` in MB; UI displays in GB
- `Machine.MemSize` is in GB (divided by 1024 from raw value)
- `UpdateMachineMemSize` receives GB and multiplies by 1024 for storage
- Extension methods: `GiBiBytesToKiBiBytes()` and `KiBiBytesToGiBiBytes()` in `DoubleExtensions`
