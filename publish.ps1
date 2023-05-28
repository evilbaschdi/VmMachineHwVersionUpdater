dotnet clean
dotnet restore
dotnet build

Set-Location VmMachineHwVersionUpdater.Wpf
.\publish.ps1

Set-Location ..

Set-Location VmMachineHwVersionUpdater.Avalonia
.\publish.ps1

Set-Location ..