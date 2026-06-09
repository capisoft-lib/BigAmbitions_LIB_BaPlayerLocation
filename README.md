# LIB_BaPlayerLocation

Big Ambitions **library mod** (EA 0.11+) that exposes live player location to other mods:

- position, heading, speed
- movement mode (`Indoor`, `Walk`, `Car`, `Subway`, `Unavailable`)
- place name (building or neighborhood when known)

Notifications fire **only on significant change** (not every frame).

| | |
|---|---|
| **Game** | Big Ambitions EA **0.11 Experimental** |
| **Unity** | **2022.3.62f2** with [Big Ambitions Modding SDK](https://github.com/hovgaardgames/bigambitions) |
| **Mod ID** | `LIB_BaPlayerLocation` |
| **Assembly** | `LIB_BaPlayerLocation` |
| **Namespace** | `BaPlayerLocation.Subscriber` |

## Install into your SDK

1. Clone this repository.
2. Copy the **contents of this repo** (or the whole folder) into your SDK:

   ```text
   <YourSdk>/Assets/Mods/LIB_BaPlayerLocation/
   ```

   Or run:

   ```powershell
   .\tools\install-into-sdk.ps1 -SdkPath "C:\path\to\bigambitions"
   ```

3. Open the SDK in Unity, import game DLLs if prompted.
4. **Mod Builder → Build & Install** for `LIB_BaPlayerLocation`.
5. Enable **LIB BA Player Location** in the in-game mod menu.

Ship this mod with any consumer mod that uses the API (players must have both enabled).

## Consumer mod setup (compile-time)

Add an asmdef reference to this library. Stable GUID:

```text
c7d8e9f0a1b243c5d6e7f8091a2b3c4d
```

In your mod's `.asmdef`:

```json
{
  "references": [
    "GUID:c7d8e9f0a1b243c5d6e7f8091a2b3c4d"
  ]
}
```

Copy `templates/Example-Consumer-Mod/` as a starting point for a new mod.

## Integration

Use `SubscribeWhenActive` in `OnLoadAsync` and dispose in `OnUnloadAsync`. It handles mod load order — your consumer mod may start before this library.

```csharp
using BaPlayerLocation.Subscriber;
using BAModAPI;
using UnityEngine;

private IDisposable _locationSubscription;

public Task OnLoadAsync(ModContext context)
{
    _locationSubscription = PlayerLocationSubscriber.SubscribeWhenActive(snapshot =>
    {
        if (!snapshot.IsAvailable)
            return;

        Debug.Log($"{snapshot.MovementKind} @ {snapshot.Position} place={snapshot.Place}");
    });

    return Task.CompletedTask;
}

public Task OnUnloadAsync()
{
    _locationSubscription?.Dispose();
    _locationSubscription = null;
    return Task.CompletedTask;
}
```

Movement mode or place changes always trigger a notification. Position, heading, and speed use the thresholds below (configurable).

## Configuration

The official SDK provides `OptionsService` for in-game mod settings (see `Example-Options`). That API uses integer sliders and is suited to player-facing toggles.

This library uses a **JSON file in `ModsLocal`** instead — same pattern as other telemetry mods, and better for sub-meter float thresholds.

Copy `subscriber_config.json.example` to:

```text
%USERPROFILE%\AppData\LocalLow\...\BigAmbitions\ModsLocal\LIB_BaPlayerLocation\subscriber_config.json
```

```json
{
  "position_threshold_m": 0.5,
  "heading_threshold_deg": 2.0,
  "speed_threshold_mps": 0.5
}
```

Omit the file to use defaults. Invalid or non-positive values fall back to defaults and are logged.

| Key | Default | Meaning |
|-----|---------|---------|
| `position_threshold_m` | 0.5 | Minimum position delta (meters) before notify |
| `heading_threshold_deg` | 2.0 | Minimum heading delta (degrees) before notify |
| `speed_threshold_mps` | 0.5 | Minimum speed delta (m/s) before notify |

Mode and place changes are always reported regardless of these thresholds.

## Advanced API

| API | Use when |
|-----|----------|
| `TryGetCurrent` | One-off read (e.g. button click), not as primary integration |
| `OnActive` | One-time setup without a subscription |
| `Subscribe` | You control timing and know the library is already active |
| `Changed` | Avoid — no `Dispose`, easy to leak handlers |

## Repository layout

```text
LIB_BaPlayerLocation.asmdef       mod assembly definition
ModManifest.asset                 Big Ambitions mod manifest
subscriber_config.json.example    threshold defaults template
Scripts/                          probe + public subscriber API
tools/install-into-sdk.ps1        copy into an SDK tree
templates/Example-Consumer-Mod    minimal consumer skeleton
```

## License

MIT — see [LICENSE](LICENSE).
