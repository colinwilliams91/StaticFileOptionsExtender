param (
    [string]$Version
)

$changelogPath = "$PSScriptRoot\..\Docs\CHANGELOG.md"
$outputPath = "$PSScriptRoot\..\Docs\CHANGELOG_PLAIN.txt"

if (Test-Path $changelogPath) {
    $content = Get-Content $changelogPath
    $contentForLinks = $content

    # Updating Docs\CHANGELOG.md with links to remote tags
    $contentForLinks = $contentForLinks -replace ';;;', "./tree/$Version"

    # Write the processed content to a new file
    Set-Content -Path $changelogPath -Value $contentForLinks -Force

    # Simple example: replace markdown headers with plain text
    $content = $content -replace '### ', '--- '
    $content = $content -replace '## ', '--- '
    $content = $content -replace '# ', '=== '

    # Write the processed content to a new file
    Set-Content -Path $outputPath -Value $content -Force
}
