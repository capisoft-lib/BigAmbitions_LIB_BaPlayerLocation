# Example consumer mod

Copy this folder to `Assets/Mods/MyMod/` in your SDK, then:

1. Rename `Example-Consumer-Mod` → your mod id (folder name must match `ModId` in manifest).
2. Generate new `.meta` files in Unity (or copy from another example mod and change GUIDs).
3. Add the asmdef reference to `LIB_BaPlayerLocation` (GUID `c7d8e9f0a1b243c5d6e7f8091a2b3c4d`).
4. Build & install **LIB_BaPlayerLocation** and your mod.
5. Enable both mods in-game.

`SubscribeWhenActive` handles load order — your mod does not need to start after the library.
