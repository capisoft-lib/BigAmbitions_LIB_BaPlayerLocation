# Example consumer mod

Copy this folder to `Assets/Mods/MyMod/` in your SDK, then:

1. Rename `Example-Consumer-Mod` → your mod id (folder name must match `ModId` in manifest).
2. Generate new `.meta` files in Unity (or copy from another example mod and change GUIDs).
3. Add a `ModManifest.asset` (copy from another example mod and update `ModId` / `DisplayName`).
4. Build & install **LIB_BaPlayerLocation** and your mod.
5. Enable both mods in-game.

`Example-Consumer-Mod.asmdef` already references the Mod API and `LIB_BaPlayerLocation`.

In `OnLoadAsync`, pass a named handler to `SubscribeWhenActive` and `Dispose()` the returned subscription in `OnUnloadAsync` — see `Scripts/ExampleConsumerMod.cs`.
