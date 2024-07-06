# Changelog
Also available at https://github.com/colinwilliams91/StaticFileOptionsExtender/blob/main/CHANGELOG.md
The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html)
## Unreleased
### Added
- Cross-platform compatibility for .NET (Windows, macOS, Linux)
## [Version 2.0.6](;;;) - 07-05-25
### Changed
- `PreprocessChangelog.ps1` `CHANGELOG` formatting to correct remote rel path
  - after tests on `nuget Pack` exec in MSBuild yield bad results (this is another test)
## [Version 2.0.5](./tree/v2.0.5) - 07-05-25:
### Changed
- `PreprocessChangelog.ps1` formats `CHANGELOG` with links to remoge release-tag branches
  - testing on pack for MSBuild exec w/ XML element arg
## [Version 2.0.4](./tree/v2.0.4) - 07-05-25:
### Fixed
- `PreprocessChangelog.ps1` string replace methods order so formatting finds correct chars and replaces
## [Version 2.0.3](./tree/v2.0.3) - 06-29-25:
### Changed
- Move CWDev icon to `Assets\`
- Try to remove `Docs/` literal from documentation filenames
## [Version 2.0.2](./tree/v2.0.5) - 06-29-25:
### Added
- CHANGELOG_PLAIN.txt to format release notes readability for nuget.org
  - unpacked scripts for preprocessing and cleanup targetting Pack
### Changed
- README readability
- DOCUMENTATION files to `\Docs` directory
## [Version 2.0.1](./tree/v2.0.1) - 06-28-25:
### Added
- README documentation on...
  - to contribute...
  - references to format and patterns for documentation (repo `CHANGELOG`)
### Changed
- README documentation on...
  - how to use for updated Namespace (`CWDev`)
## [Version 2.0.0](./tree/v2.0.0) - 06-28-25:
### Added
- Release notes to `nuspec`
- Git tags to release commits
### Changed
- Renamed namespace from `StaticFileOptionsExtender` to `CWDev` to remove redundancy in `using static` statement
## [Version 1.0.2](./tree/v1.0.2) - 06-28-24:
### Changed
- README documentation on...
	- how to use...
## [Version 1.0.0](./tree/v1.0.0) - 06-28-24:
### Added
- `StaticFileOptionsExtender` namespace
- `StaticFileOptionsExtender` static class
- Namespace Icon
- License
- README documentation on...
	- purpose...
	- abstract...
