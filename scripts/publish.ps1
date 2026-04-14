# This script is used to publish the App for different runtimes.
# It sets the target framework to .NET 10.0 and specifies the runtimes for x64 and ARM64 architectures.

$targetFramework = "net10.0"
$runtimes = @("win-x64", "win-arm64")
$appsDirectory = "C:\Apps"
$appName = "VmMachineHwVersionUpdater"
$projectPath = "src/VmMachineHwVersionUpdater.Avalonia/VmMachineHwVersionUpdater.Avalonia.csproj"
$outputBase = "$appsDirectory\$appName"

# Clean, restore, and build the solution
dotnet clean
dotnet restore
dotnet build

# Copy AppLauncher and rename it to the app name
$appLauncherSource = "$appsDirectory\AppLauncher\x64\AppLauncher.exe"
$appLauncherTarget = "$appsDirectory\$appName\$appName.exe"

foreach ($runtime in $runtimes) {
    Write-Output "Publishing for $runtime..."
    dotnet publish $projectPath -c Release -o "$outputBase\$($runtime.Replace('win-', ''))" -r $runtime -f $targetFramework --no-self-contained
}

if (Test-Path $appLauncherSource) {
    Write-Output "Copying AppLauncher to $appLauncherTarget..."
    if (!(Test-Path "$appsDirectory\$appName")) {
        New-Item -ItemType Directory -Path "$appsDirectory\$appName" -Force
    }
    Copy-Item -Path $appLauncherSource -Destination $appLauncherTarget -Force
    Write-Output "Launcher ready: $appLauncherTarget"
}
else {
    Write-Warning "AppLauncher not found at $appLauncherSource"
}