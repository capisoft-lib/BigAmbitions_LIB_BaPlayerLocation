# Changelog

## [1.0.0] - 2026-08-26

### Changed

- First stable 1.0 release of the shared player-location library
- The same assembly and subscriber API are verified for Big Ambitions EA 0.11 and 1.0 experimental
- Release packaging is produced through the official Unity 2022.3.62f2 / Big Ambitions Mod Builder pipeline
- Public subscriber API and assembly identity remain unchanged for existing consumer mods

## [0.11.2] - 2026-07-25

### Fixed

- **Flatbed & hand truck classification**: vehicles with `spawnInPlayerObject` are reported as `Walk`, not `Car`
- Consumer mods can no longer mistake pushed cargo tools for motor vehicles and teleport them through auto-drive actions
- Player position and heading are used while pushing delivery cargo

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

[1.0.0]: https://github.com/capisoft-lib/BigAmbitions_LIB_BaPlayerLocation/releases/tag/v1.0.0
[0.11.2]: https://github.com/capisoft-lib/BigAmbitions_LIB_BaPlayerLocation/releases/tag/v0.11.2
[0.11.1]: https://github.com/capisoft-lib/BigAmbitions_LIB_BaPlayerLocation/releases/tag/v0.11.1
[0.11.0]: https://github.com/capisoft-lib/BigAmbitions_LIB_BaPlayerLocation/releases/tag/v0.11.0
