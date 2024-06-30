param (
    [string]$v,
    [string]$apiKey
)

# Construct the NuGet package file path
$releasePath = ".\bin\Release"
$packageFileName = "\CWDev.StaticFileOptionsExtender.$v.nupkg"
$packageFullPath = $releasePath + $packageFileName

# Push the NuGet package using dotnet nuget push command
dotnet nuget push $packageFullPath --api-key $apiKey --source https://api.nuget.org/v3/index.json
