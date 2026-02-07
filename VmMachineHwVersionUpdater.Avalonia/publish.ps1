# This script is used to publish the App for different runtimes.
# It sets the target framework to .NET 10.0 and specifies the runtimes for x64 and ARM64 architectures.
$targetFramework = "net10.0"
$runtimes = @("win-x64", "win-arm64")
$appsDirectory = "C:\Apps"
$appName = (Get-Item .).Name
$outputBase = "$appsDirectory\$appName"

foreach ($runtime in $runtimes) {
    $arch = $runtime.Replace('win-', '')
    Write-Host "Publishing for $runtime..." -ForegroundColor Cyan

    dotnet publish `
        -c Release `
        -r $runtime `
        -f $targetFramework `
        --self-contained true `
        -p:PublishAot=true `
        -p:PublishTrimmed=true `
        -o "$outputBase\$arch"
}

# Launcher Logik
$appLauncherSource = "$appsDirectory\AppLauncher\x64\AppLauncher.exe"
$appLauncherTarget = "$outputBase\$appName.exe" # Ziel im Hauptordner

if (Test-Path $appLauncherSource) {
    if (!(Test-Path $outputBase)) { New-Item -ItemType Directory -Path $outputBase }
    Copy-Item -Path $appLauncherSource -Destination $appLauncherTarget -Force
    Write-Host "Launcher ready: $appLauncherTarget" -ForegroundColor Green
}
else {
    Write-Warning "AppLauncher not found at $appLauncherSource"
}