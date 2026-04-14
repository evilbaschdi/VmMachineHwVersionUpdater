---
applyTo: "**/*.{sln,slnx,csproj,props,targets}"
---

# .NET-Projektstruktur (Fowl/dotnet-template-Konvention)

Beim Erstellen, Umstrukturieren oder Überprüfen von .NET-Projekten folge dieser Verzeichniskonvention
(basierend auf https://gist.github.com/davidfowl/ed7564297c61fe9ab814
und https://github.com/dotnet-template/project-layout):

## Zielstruktur

```
$/
  artifacts/        # Build-Outputs (nupkgs, dlls, pdbs) — nur in .gitignore, nicht committen
  build/            # Build-Customizations (custom MSBuild-Targets, CI-Skripte)
  deployments/      # IaaS, PaaS, Container-Orchestrierung (docker-compose, k8s, terraform)
  docs/             # Dokumentation (Markdown, Hilfe-Dateien)
  lib/              # Abhängigkeiten die NICHT als NuGet-Paket existieren können
  packages/         # NuGet-Pakete — nur in .gitignore, nicht committen
  samples/          # Beispielprojekte (optional)
  scripts/          # Utility-Skripte (Publish, Install, Analyse, Migrations)
  src/              # Produktcode — alle Hauptprojekte
  tests/            # Testprojekte (Unit, Integration)
  .editorconfig     # Cross-Platform IDE-Einstellungen
  .gitattributes    # Git-Attribute
  .gitignore        # Git-Ignore-Regeln
  build.cmd         # Build-Bootstrapper (Windows)
  build.sh          # Build-Bootstrapper (*nix)
  global.json       # .NET SDK-Version
  LICENSE           # Lizenz (bei OSS-Projekten)
  NuGet.Config      # NuGet-Paketquellen
  README.md         # Projekt-Beschreibung
  {solution}.sln    # Solution-Datei im Root (klassisches Format)
  {solution}.slnx   # Solution-Datei im Root (neues XML-Format, ab .NET 9)
```

## Regeln zur Umstrukturierung

### Projekte verschieben

- Alle Produktcode-Projekte (Libraries, Apps, APIs, Worker) → `src/`
- Alle Testprojekte (`*.Tests`, `*.IntegrationTests`, `*.Benchmarks`) → `tests/`
- Projektordner behalten ihren Namen (z.B. `src/MyApp.Core/`, `tests/MyApp.Core.Tests/`)

### Skripte verschieben

- `publish.ps1` und andere Utility-Skripte aus dem Root oder aus Projektordnern → `scripts/`
- **Pfadanpassungen:** Pfade innerhalb der Skripte (z. B. zu `.csproj`-Dateien) müssen zwingend auf die neue Struktur angepasst werden.
  - Skripte im `scripts/`-Ordner referenzieren Projekte meist via `..\src\ProjectName\ProjectName.csproj`.
  - Bestehende Skripte in Unterordnern (z. B. `src/ProjectName/`) müssen ebenfalls aktualisiert werden, wenn sie auf verschobene Abhängigkeiten oder Verzeichnisse verweisen.

### Solution-Datei anpassen (.sln oder .slnx)

- Es kann sich um das klassische `.sln`-Format oder das neue XML-basierte `.slnx`-Format handeln
- `.slnx`: Projektpfade in `<Project Path="..." />`-Elementen aktualisieren,
  Solution-Ordner über `<Folder Name="/src/">` und `<Folder Name="/tests/">` anlegen
- `.sln`: Projektpfade in `Project(...)`-Einträgen und `SolutionFolder`-GUIDs aktualisieren
- Solution-Items (`Directory.Build.props`, `global.json`, `NuGet.Config`) bleiben im Root

### IDE- und Tool-Konfigurationen anpassen

- **.vscode/**: Pfade in `launch.json` (z. B. `program`, `cwd`) und `tasks.json` (z. B. `args` beim `dotnet build`) auf die neuen Projektstandorte in `src/` oder `tests/` anpassen.
- **.idea/**: Falls vorhanden, Projektzuordnungen in `.idea`-Ordnern (z. B. Rider/Resharper-Settings) prüfen.
- **Andere Tools**: Pfade in `.runsettings`, `benchmarkdotnet`-Konfigurationen oder ähnlichen Tool-Dateien aktualisieren.

### ProjectReference-Pfade in .csproj

- Projekte innerhalb von `src/` referenzieren sich gegenseitig über `..\..\`-relative Pfade
  (z.B. `..\MyApp.Core\MyApp.Core.csproj`)
- Testprojekte in `tests/` referenzieren `src/`-Projekte über
  `..\..\src\{Projekt}\{Projekt}.csproj`

### Abgrenzung `build/` vs. `scripts/`

- **`build/`** → Dateien die den Build-Prozess selbst anpassen: custom MSBuild-Targets,
  `.props`/`.targets`-Dateien, CI-Pipeline-Definitionen (YAML), Cake/FAKE-Skripte
- **`scripts/`** → Ausführbare Skripte für konkrete Operationen:
  `publish.ps1`, `migrate.ps1`, `setup.sh`, Analyse-Skripte etc.

### Verzeichnisse anlegen

- `src/` und `tests/` — immer
- `docs/`, `build/`, `samples/`, `scripts/`, `deployments/`, `lib/` — nur bei Bedarf
- `artifacts/` und `packages/` — NICHT anlegen, nur in `.gitignore` eintragen

### Root-Dateien

- `Directory.Build.props`, `Directory.Packages.props`, `global.json`, `NuGet.Config` → Root
- `.editorconfig` → Root (sicherstellen, dass vorhanden)
- `.gitignore` → Root, muss mindestens enthalten:
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

### Validierung

- Nach Umstrukturierung: `dotnet build` → 0 Fehler
- Nach Umstrukturierung: `dotnet test` → keine neuen Fehler
- CI/CD-Pipelines auf geänderte Pfade prüfen

### Was NICHT geändert wird

- Projektinhalte (*.cs, *.axaml etc.) — nur verschieben, nicht modifizieren
- PackageReference-Einträge — nur ProjectReference-Pfade anpassen
- Namespaces — bleiben unverändert (sofern nicht explizit gewünscht)
