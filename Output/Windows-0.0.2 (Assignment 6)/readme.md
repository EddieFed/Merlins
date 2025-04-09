# Assignment 6
All work is in scene "Scene99 - Eddie Test"
## Slime AI and Mecanim
Contributed by Daniel Barajas
- The slime monster fits the magical theme of the game and makes it harder for the player to sell products to customers by eating their stock on the shelves
- The Slime AI is implemented using an FSM, changing from states like Idle, Moving, Eating, and Dead
- The Slime Mecanim has animations for idling, walking, eating stock, and dying
- Slimes will navigate to a random shelf using a navmesh, and once reaching the shelf, they will begin slowly eating the stock
- Players can stop the slime by getting close enough to step on the slime, triggering its death
- Slime Model and Animations
  https://assetstore.unity.com/packages/3d/characters/creatures/rpg-monster-duo-pbr-polyart-157762

## Dragon AI and Mecanim
Contributed by Eddie Federmeyer
- Bayesian Neural Network controlled Dragon
- Simple 4x4 network to begin with, will eventually take most, if not all environmental factors in
- Finds a local NPC and follows them. Changes this probabilistically depending on distance and speed of the target
- Built using Infer.NET, a Probabilistic Programming library by Microsoft
- Dragon Model and Animations from: https://assetstore.unity.com/packages/p/battle-dragon-axe-63644 

## Flocking AI
Contributed by Adrian Knight
- FlockingBatManager controls flocking set of a flies
- Flies in the flock will move towards average position of its neighbors while keeping a certain distance giving a intelligent swarm display
- The manager is also able to set a hard bounds that flies will always turn in from and a different goal positions to make swarm movement dynamic
- Cashier and Chest monster assets: https://assetstore.unity.com/packages/3d/characters/creatures/rpg-monster-partners-pbr-polyart-168251

## Lighting
Contributed by Adrian Knight
- Store scene has bright sunlight as the store operates during the day
- Register screens have blue point lights
- Fly swarm has green point light at center to convey negative effect on customers and store.
