# This script is used to publish the App for different runtimes.
# It sets the target framework to .NET 9.0 and specifies the runtimes for x64 and ARM64 architectures.
$targetFramework = "net9.0"
$runtimes = @("win-x64", "win-arm64")
$outputBase = "C:\Apps\$((Get-Item .).Name)"

foreach ($runtime in $runtimes) {
    dotnet publish -c Release -o "$outputBase\$($runtime.Replace('win-', ''))" -r $runtime -f $targetFramework --no-self-contained
}