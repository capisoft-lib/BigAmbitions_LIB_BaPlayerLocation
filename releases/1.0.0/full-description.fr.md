[b]LIB BA Player Location 1.0.0[/b]

Bibliothèque partagée fournissant la position du joueur et son état de déplacement aux mods Big Ambitions. Le même paquet 1.0.0 prend en charge Big Ambitions EA 0.11 et la version 1.0 expérimentale.

Il s’agit d’une dépendance utilisée par des mods tels que Voogle Route. Abonnez-vous à cette bibliothèque et activez-la dans le menu [b]Mods[/b] du jeu lorsqu’un autre mod indique que [b]LIB BA Player Location[/b] est requise.

[b]Nouveautés de la version 1.0.0[/b]

[list]
[*][b]Paquet compatible avec les deux versions[/b] — un seul assembly stable pour EA 0.11 et la version 1.0 expérimentale
[*][b]API stable pour les mods utilisateurs[/b] — les mods existants conservent la même identité d’assembly, le même espace de noms et le même contrat d’intégration
[*][b]Détection fiable des déplacements[/b] — les chariots à plateau et les diables restent classés comme déplacement à pied plutôt que comme véhicules motorisés
[*][b]Processus de compilation officiel[/b] — paquet créé avec Unity 2022.3.62f2 et le Mod Builder de Big Ambitions
[/list]

[b]Pour les développeurs de mods[/b]

Utilisez l’API destinée aux mods clients pour recevoir la position, l’orientation, la vitesse, le lieu et le type de déplacement du joueur sans que chaque mod interroge séparément les mécanismes internes du jeu. Les mods clients doivent référencer l’assembly partagé [code]LIB_BaPlayerLocation[/code] et ne doivent pas en inclure une copie privée.

[b]Soutenir le développeur ☕[/b]

Si cette bibliothèque a aidé vos mods préférés à retrouver l’endroit où le joueur s’était encore égaré, vous pouvez soutenir son développement en m’offrant un café :

[url=https://buymeacoffee.com/capitaine]☕ M’offrir un café[/url]

Les coordonnées restent précises ; sans café, le développeur l’est un peu moins.
