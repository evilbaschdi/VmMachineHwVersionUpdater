# Clean, restore, and build the solution
dotnet clean
dotnet restore
dotnet build

# Define the subDirectories to publish
$subDirectories = @("VmMachineHwVersionUpdater.Avalonia")

# Iterate over each subDirectory and run its publish script
foreach ($subDirectory in $subDirectories) {
    Write-Output "Start publishing '$subDirectory'..."
    Set-Location $subDirectory
    .\publish.ps1
    Set-Location ..
}