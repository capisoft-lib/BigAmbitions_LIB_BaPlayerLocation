[b]LIB BA Player Location 1.0.0[/b]

Shared player-location and movement-state library for Big Ambitions mods. The same 1.0.0 package supports Big Ambitions EA 0.11 and 1.0 experimental.

This is a dependency used by mods such as Voogle Route. Subscribe to it and enable it in the in-game [b]Mods[/b] menu when another mod lists [b]LIB BA Player Location[/b] as required.

[b]What's new in 1.0.0[/b]

[list]
[*][b]Cross-version package[/b] — one stable library assembly for EA 0.11 and 1.0 experimental
[*][b]Stable subscriber API[/b] — existing consumer mods keep the same assembly identity, namespace and integration contract
[*][b]Movement safety[/b] — flatbeds and hand trucks remain classified as walking rather than motor vehicles
[*][b]Official build pipeline[/b] — packaged through Unity 2022.3.62f2 and the Big Ambitions Mod Builder
[/list]

[b]For mod developers[/b]

Use the subscriber API to receive player position, heading, speed, place and movement kind without probing game internals independently. Consumer mods must reference the shared [code]LIB_BaPlayerLocation[/code] assembly and must not bundle a private copy.

[b]Support the developer ☕[/b]

If this library has helped your favorite mods find where the player wandered off to, you can support its development by buying me a coffee:

[url=https://buymeacoffee.com/capitaine]☕ Buy me a coffee[/url]

The coordinates stay precise; the developer is less reliable without caffeine.
