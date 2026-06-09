# Changelog

## [0.11.1] - 2026-06-09

### Fixed

- **Steam Workshop paths**: `subscriber_config.json` and shipped example use `ModContext.ModRootPath` only
- Content vs user config split: example ships with the mod; writable config lives in the mod root

### Changed

- `ModStoragePaths` centralizes all runtime file paths

## [0.11.0] - 2026-06-07

### Added

- Initial **LIB BA Player Location** library mod for Big Ambitions EA 0.11
- `PlayerLocationSubscriber` API for consumer mods
- JSON threshold configuration (`subscriber_config.json`)
- Example consumer mod template

[0.11.1]: https://github.com/capisoft-lib/BigAmbitions_LIB_BaPlayerLocation/releases/tag/v0.11.1
[0.11.0]: https://github.com/capisoft-lib/BigAmbitions_LIB_BaPlayerLocation/releases/tag/v0.11.0
