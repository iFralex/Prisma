# Prisma - 2D Multiplayer Game

**Overview:**
Prisma is a 2D multiplayer game that draws inspiration from Minecraft, offering an immersive gaming experience with a focus on tile-based world interaction, crafting, and online multiplayer capabilities. This technical description delves into the various aspects of the game, highlighting key features, technologies, and development details.

![Prisma gameplay](https://github.com/iFralex/Prisma/assets/61825057/09b68eba-bc26-438d-b752-fc7106dff682)


## Key Features:

- **Tile-Based World Interaction:** The game is built on a tile-based world, allowing players to interact with and manipulate tiles. These tiles represent the 2D world inspired by Minecraft, and they provide a dynamic and visually appealing environment.

- **Crafting System:** The game's crafting system is still in its early stages of development. Players can craft a limited range of items, with ongoing work to expand this feature.

- **Multiplayer Functionality:** Multiplayer support is a core component of Prisma, enabling players to collaborate or compete in real-time. The game's multiplayer functionality is implemented using PUN 2 (Photon Unity Networking 2), a widely used framework for Unity game development.

- **Advanced AI (Zombies):** The game features formidable AI-controlled enemies in the form of zombies, with advanced pathfinding capabilities. The A* Pathfinding Project is used to ensure that zombies navigate the game world efficiently and engage players in challenging combat scenarios.

- **Tile Effects:** Each tile in the game world has unique properties that affect gameplay. For example, players can encounter obstacles such as walls that block their path or benefit from faster movement on certain tile types like paths.

- **Player Stats Management:** Prisma includes player stat management, affecting various aspects of gameplay. Hunger, power, and health stats influence a player's survival and combat abilities, adding depth to the gaming experience.

- **Economy and Virtual Shop:** Players can participate in a virtual economy within the game. They have the ability to buy and sell items in a virtual shop, setting their own prices and facilitating trade with other players.

- **Creative Mode:** For creative and sandbox-style play, Prisma offers a creative mode. In this mode, players can unleash their creativity without restrictions, building and experimenting to their heart's content.

## Technical Details:

- **Unity Engine:** Prisma is developed using the Unity game engine, leveraging its versatile capabilities for 2D game development.

- **C# Programming:** The game's core logic is written in C#, a popular language for Unity development. This includes character movement, combat mechanics, crafting systems, and more.

- **Networking:** Multiplayer functionality is implemented using [PUN 2 (Photon Unity Networking 2)](https://www.photonengine.com/pun#). This framework enables seamless online interactions, ensuring that players can explore, collaborate, and compete in real time.

- **AI Pathfinding:** The game's advanced AI is powered by the [A* Pathfinding Project](https://assetstore.unity.com/packages/tools/behavior-ai/a-pathfinding-project-pro-87744#content) package, utilizing the [A* algorithm](https://en.wikipedia.org/wiki/A*_search_algorithm) for intelligent enemy movement and navigation. This ensures that AI-controlled zombies can efficiently track and engage with players.

- **Google Sheets Integration:** The game's backend uses Google Sheets using the [Google Sheets for Unity](https://novack.itch.io/google-sheets-for-unity) package to store and manage tile data. While the main Google Sheets document may no longer be accessible, the game's code and logic remain functional, and alternative new sheet document could be created.

- **Development Journey:** This project has been commissioned by a significant client, representing a substantial accomplishment in game development. It has provided the developer with invaluable insights into C# programming, Unity, and game design.

