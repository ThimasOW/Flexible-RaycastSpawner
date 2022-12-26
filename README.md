# Flexible-RaycastSpawner

This spawner has the capability of spawning random types of prefabs at runtime and spawning select types every x seconds.
Attach this script as a component to a gameObject and tweak the parameters to your solution. 

The RaycastSpreader can be visualized as an invisible plane above the terrain. The dimensions of this plane can be changed
using the item"X,Y,Z"Spread variables. When it finds a suitable spot on the terrain, it will spawn a PhysicsOverlapBox at that hit,
and check if its boundaries are free from gameObjects using the layers in spawnedObjectLayer.

It will also place the prefabs at a correct angle based on the geometry of the terrain. The slope of the terrain can also
be constrained in the Spawn() method using EulerAngles, if the objects should not be able to spawn at e.g. a 90 degree slope. 

Example configuration:
![image](https://user-images.githubusercontent.com/79156616/209563651-f8bd3463-d7e6-4ca5-872b-18faa3a24912.png)
Note: A singleton List of animal prefabs were used in this example

Result:
![image](https://user-images.githubusercontent.com/79156616/209563774-480ed7ee-a07a-442c-a9b9-c5ac3398c019.png)
Debug.DrawLine used to visualize spawns.
