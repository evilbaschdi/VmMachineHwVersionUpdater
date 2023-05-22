dotnet clean
dotnet restore
dotnet build
dotnet publish -c Release -o "C:\Apps\$((Get-Item .).Name)\x64" -r win-x64 -f net7.0-windows --no-self-contained
dotnet publish -c Release -o "C:\Apps\$((Get-Item .).Name)\arm64" -r win-arm64 -f net7.0-windows --no-self-contained
