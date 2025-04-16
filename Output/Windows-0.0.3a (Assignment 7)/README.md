# Assignment 7

Adrian Knight
- Added buzzing sound to fly flock
- Flock now targets the first customer that any flock member touches first
  - Flock chases that customer till outside of its chase limit
- The entire flock dies when only DeathLimit percent remains
- Flock members die when the player collides with them
- Flock swarm respawns on timer after full death
- Layout customer flee to exit logic when taking too much damage from enemies

Daniel Barajas
- Added basic game logic to keep gameplay cohesive and help other gameplay elements work together seamlessly
  - Stages of the game (Setup, Shop is open, Shop is closing, Game is done)
  - NPCs can interact with other elements such as shelves
- Added UI elements for players to see information about the game to better understand what’s happening
  - A timer shows the player how much time has passed and when the round will end
  - A customer counter shows how many customers are currently in the shop
  - A bank counter keeps track of how much money has been made (how well the player is doing)
  - A held item display shows what color of item the player is holding (they can only restock shelves of a matching color)
- Added various background music assets to our sound manager that will play at different points of the game
  - Lobby music, music played during the game, and music for after the shop has closed and the player is waiting for all customers to leave

Eddie Federmeyer
- Added new level design with more intuitive and well understood models (shelves, tile, stock, etc.)
- Supplied sounds effects
  - Slimes make a noticeable sound when spawning, dying and eating
  - Players make a “thump” sound when restocking a shelf
- Fixed user movement so that the player now visually shows the direction they are moving in, allowing for easier navigation with the orthogonal camera angle
