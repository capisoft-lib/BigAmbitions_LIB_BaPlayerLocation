[b]LIB BA Player Location[/b]

Shared player location and movement-state library for Big Ambitions mods.

This is a dependency used by mods such as Voogle Route. Subscribe to it and enable it in the in-game Mods menu when another mod lists it as required.

[b]What's new in 0.11.2[/b]

[list]
[*][b]Flatbed & hand truck safety[/b] — pushed delivery equipment is now reported as walking, not driving
[*][b]Correct player pose[/b] — consumer mods receive the player's position and heading while cargo is being pushed
[*][b]Auto-drive protection[/b] — prevents dependent mods from treating delivery carts as motor vehicles
[/list]

[b]For mod developers[/b]

Use the included subscriber API to receive player position, heading, speed and movement kind without probing game internals independently.
