---
applyTo: "**/*.{sln,slnx,csproj,props,targets}"
---

# .NET Project Structure (Fowl/dotnet-template Convention)

When creating, restructuring, or reviewing .NET projects, follow this directory convention 
(based on https://gist.github.com/davidfowl/ed7564297c61fe9ab814 
and https://github.com/dotnet-template/project-layout):

## Target Structure

```
$/
  artifacts/        # Build outputs (nupkgs, dlls, pdbs) — in .gitignore only, do not commit
  build/            # Build customizations (custom MSBuild targets, CI scripts)
  deployments/      # IaaS, PaaS, container orchestration (docker-compose, k8s, terraform)
  docs/             # Documentation (Markdown, help files)
  lib/              # Dependencies that CANNOT exist as a NuGet package
  packages/         # NuGet packages — in .gitignore only, do not commit
  samples/          # Sample projects (optional)
  scripts/          # Utility scripts (publish, install, analysis, migrations)
  src/              # Product code — all main projects
  tests/            # Test projects (Unit, Integration)
  .editorconfig     # Cross-platform IDE settings
  .gitattributes    # Git attributes
  .gitignore        # Git ignore rules
  build.cmd         # Build bootstrapper (Windows)
  build.sh          # Build bootstrapper (*nix)
  global.json       # .NET SDK version
  LICENSE           # License (for OSS projects)
  NuGet.Config      # NuGet package sources
  README.md         # Project description
  {solution}.sln    # Solution file in root (classic format)
  {solution}.slnx   # Solution file in root (new XML format, from .NET 9)
```

## Restructuring Rules

### Moving Projects

- All product code projects (Libraries, Apps, APIs, Workers) → `src/`
- All test projects (`*.Tests`, `*.IntegrationTests`, `*.Benchmarks`) → `tests/`
- Project folders keep their name (e.g., `src/MyApp.Core/`, `tests/MyApp.Core.Tests/`)

### Moving Scripts

- `publish.ps1` and other utility scripts from root or project folders → `scripts/`
- **Path Adjustments:** Paths within scripts (e.g., to `.csproj` files) MUST be adjusted to the new structure.
  - Scripts in the `scripts/` folder usually reference projects via `..\src\ProjectName\ProjectName.csproj`.
  - Existing scripts in subfolders (e.g., `src/ProjectName/`) must also be updated if they reference moved dependencies or directories.

### Publishing Configuration (`scripts/`)

When using central publishing scripts, the following files must be maintained:

- **`scripts/publish.json`**: Configuration file for the publishing process.
  - Every new executable project must be entered here.
  - Fields:
    - `project`: Name of the project (matches folder name in `src/`).
    - `runtimes`: List of target runtimes (e.g., `["win-x64", "win-arm64"]`).
    - `targetFramework`: .NET version (e.g., `net10.0`).
    - `selfContained`: `true` or `false`.
    - `withAppLauncher`: Whether the `AppLauncher` should be used.
- **`scripts/publish.ps1`**: PowerShell script to execute build and publish.
  - The script references projects relative to its location via `..\src\{ProjectName}\{ProjectName}.csproj`.
  - It ensures that binaries are copied to `C:\Apps\{ProjectName}\{Runtime}`.

### Adjusting Solution Files (.sln or .slnx)

- This can be the classic `.sln` format or the new XML-based `.slnx` format.
- `.slnx`: Update project paths in `<Project Path="..." />` elements, 
  create solution folders via `<Folder Name="/src/">` and `<Folder Name="/tests/">`.
- `.sln`: Update project paths in `Project(...)` entries and `SolutionFolder` GUIDs.
- Solution items (`Directory.Build.props`, `global.json`, `NuGet.Config`) stay in the root.

### Adjusting IDE and Tool Configurations

- **.vscode/**: Adjust paths in `launch.json` (e.g., `program`, `cwd`) and `tasks.json` (e.g., `args` for `dotnet build`) to the new project locations in `src/` or `tests/`.
- **.idea/**: If present, check project mappings in `.idea` folders (e.g., Rider/ReSharper settings).
- **Other Tools**: Update paths in `.runsettings`, `benchmarkdotnet` configurations, or similar tool files.

### ProjectReference Paths in .csproj

- Projects within `src/` reference each other via `..\..\` relative paths 
  (e.g., `..\MyApp.Core\MyApp.Core.csproj`).
- Test projects in `tests/` reference `src/` projects via 
  `..\..\src\{Project}\{Project}.csproj`.

### Separation of `build/` vs. `scripts/`

- **`build/`** → Files that customize the build process itself: custom MSBuild targets, 
  `.props`/`.targets` files, CI pipeline definitions (YAML), Cake/FAKE scripts.
- **`scripts/`** → Executable scripts for concrete operations: 
  `publish.ps1`, `migrate.ps1`, `setup.sh`, analysis scripts, etc.

### Creating Directories

- `src/` and `tests/` — always
- `docs/`, `build/`, `samples/`, `scripts/`, `deployments/`, `lib/` — only as needed
- `artifacts/` and `packages/` — DO NOT create, only add to `.gitignore`

### Root Files

- `Directory.Build.props`, `Directory.Packages.props`, `global.json`, `NuGet.Config` → Root
- `.editorconfig` → Root (ensure it exists)
- `.gitignore` → Root, must at least contain:
  ```
  [Oo]bj/
  [Bb]in/
  .nuget/
  _ReSharper.*
  packages/
  artifacts/
  *.user
  *.suo
  *.userprefs
  *DS_Store
  *.sln.ide
  .vs/
  ```

### Validation

- After restructuring: `dotnet build` → 0 errors
- After restructuring: `dotnet test` → no new errors
- Check CI/CD pipelines for changed paths

### What is NOT changed

- Project contents (*.cs, *.axaml etc.) — only move, do not modify
- PackageReference entries — only adjust ProjectReference paths
- Namespaces — remain unchanged (unless explicitly requested)
