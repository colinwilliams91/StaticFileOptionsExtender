$changelogPath = "$PSScriptRoot\..\Docs\CHANGELOG.md"
$outputPath = "$PSScriptRoot\..\Docs\CHANGELOG_PLAIN.txt"

if (Test-Path $changelogPath) {
    $content = Get-Content $changelogPath

    # Simple example: replace markdown headers with plain text
    $content = $content -replace '### ', '--- '
    $content = $content -replace '## ', '--- '
    $content = $content -replace '# ', '=== '

    # Write the processed content to a new file
    Set-Content -Path $outputPath -Value $content -Force
}
