dotnet publish -c Release -o "C:\Apps\$((Get-Item .).Name)\x64" -r win-x64 -f net8.0-windows --no-self-contained
dotnet publish -c Release -o "C:\Apps\$((Get-Item .).Name)\arm64" -r win-arm64 -f net8.0-windows --no-self-contained
