# forTubi

Code Sample

# Exploring Realistic Natural Terrain in 3D Game Environments with Unity

## Introduction

This was the final project for my Computer Graphics class, with the goal of creating realistic terrains in a first-person game environment using Unity — without relying on premade assets. My partner and I aimed to explore how to utilize our knowledge of meshes, shaders, and colors to replicate natural environments like deserts, forests, and meadows. This project aimed to achieve visually compelling results while minimizing CPU usage and avoiding the need for time-intensive artistic renderings in tools like Blender or Maya.

The result is a system that generates distinct terrain environments, each incorporating elements like water, foliage, and varying geology. Players can navigate through these terrains using a first-person camera.

In this project, I was in charge of and wrote the code implementing the First-person Camera/Movement feature and the Water feature, while my partner was in charge of all the other features (Topgraphy/Grass/Tree Creation). As such, after the Overview, I will only include the writeup for the features worked on solely by me.

## Overview

The repository contains two main executable scenes:

    Desert and Forest Terrain (unfortunately this executable is not currently working)

    Meadow with a Pool of Water

Process Overview

Our process involved several iterative steps and learning phases, as detailed below:

    - Learn Unity

        - Set up Unity on both Windows and Mac.

        - Familiarized ourselves with Unity’s core features.

    - First-Person Camera/Movement

        - Created a first-person camera and added keyboard input controls for movement.

    - Topography Creation

        - Developed ground with varying altitudes and applied multiple terrain textures (sand, gravel, and soil).

    - Water Implementation

        - Integrated the Universal Render Pipeline (URP) and developed a custom water shader.

    - Grass Creation

        - Designed basic cartoon-style grass, then enhanced it with wind simulation, color variations, and multiple textures for a more realistic appearance.

    - Tree Creation

        - Designed trees from scratch, including palm and bamboo varieties, using Unity's Tree asset and custom textures.

Controls

    - WASD / Arrow Keys: Control horizontal movement of the player and camera.

    - Spacebar: Jump.

    - Mouse: Control camera orientation and direction.

    - ESC: Exit the game.

## First-person Camera/Movement

The first-person camera was implemented by creating both a `CameraHolder` and a `Player` object, with the camera being located within the "head" of the player object. I created three child objects for the player object:

- A capsule mesh to represent the player body
- Two transform child objects to represent the camera orientation and position.

For the `CameraHolder`, it includes one child: the actual camera object.

I then created three C# scripts to control the movement of the camera and player object:

## 1. move_camera Script

The `move_camera` script is applied to the `CameraHolder` and ensures that the camera's transform position is always aligned with the `cameraPosition` child of the player object, so the camera moves along with the player.

## 2. first_person_camera Script

The `first_person_camera` script is applied to the camera object. It locks the mouse to the center of the screen and makes it invisible. The script then takes the mouse's x and y inputs and uses them to transform the rotation of both the camera's transform component and the orientation. This allows the player to control the direction/angle the camera and player are facing through mouse movement.

## 3. xy_movement Script

The `xy_movement` script is attached to the Player object and handles the movement of the player and camera. It works by:

- Taking the horizontal and vertical inputs from the WASD or arrow keys (which will always be -1, 0, or 1).
- Multiplying those inputs by the forward and right directions of the player (stored in the `orientation` child).
- Adding the resulting values together and normalizing them.
- Multiplying the normalized value by a developer-defined `moveSpeed` to control the movement speed.
- Applying the transformed values to the rigidbody of the Player object using `AddForce`.

Additionally, an `airMultiplier` is applied if the player is in the air, which simulates different movement speeds for when the player is grounded versus when they are airborne.

The script also checks whether the Player object is on the ground by sending a vector slightly larger than half the height of the Player downwards and checking if it hits the `whatIsGround` layer (assigned to the terrain/ground). If it hits this layer, the `grounded` boolean is set to true; otherwise, it's false.

This `grounded` variable is checked when deciding if a player is allowed to jump, only allowing it when the player is grounded and after the `Cooldown` time the developer inputs has elapsed. The jump features checks for an input from the Spacebar key, if the Player is grounded, and if it is `readyToJump` (meaning the Cooldown time has passed), then adds an impulse force to the Player’s rigidbody directly in the y direction defined by the transform child, with the `jumpForce` being set by the developer.

## Additional Tweaks

This first-person camera was implemented based on Dave / GameDevelopment’s [First Person Camera tutorial](https://www.youtube.com/watch?v=f473C43s8nE) with some additional tweaks:

- The height of the Player object was increased to see above the grass created for the ground.
- I set the maximum and minimum angles for the camera and orientation to 89 and -89 degrees, respectively, to prevent clipping past the grass texture into the terrain when looking downward.

# Water

A simple water implementation was created by following Brackeys’ [Simple Water tutorial](https://www.youtube.com/watch?v=Vg0L9aCRWPE). This water was created using the Universal Render Pipeline (URP), which was installed into our existing project by following Unity’s [online documentation](https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@11.0/manual/index.html).

## Shader and Material Creation

I created a new `CartoonWater` shader and a `CartoonWater` material that used this shader. Using Unity’s Shader Graph Window and UI, I designed a material that, when applied to a plane mesh, resembles the surface of a simple depiction of water.

### Voronoi Node for Ripple Effect

The effect was achieved by creating a **Voronoi node**, which generates a moving pattern. This pattern is fed variables that control the ripple speed (which is based on a time variable), color, and density. The output of the Voronoi node is then passed into the **Base Color** of the material, giving the appearance of water’s surface.

### Gradient Noise Node for Wave Motion

A separate **Gradient Noise node** was created, which generates another moving pattern. The movement of this pattern is driven by time and an inputted `WaveSpeed` variable. The Gradient Noise is then multiplied by the plane’s normal vector (which points directly upwards since the plane is parallel to the horizontal world plane).

This result is added to the plane’s position and applied to the final position of the object node. This process effectively moves parts of the plane vertically in a wave-like motion, simulating waves on the surface of water.

# Issues

Initially, we tried to implement both the water and the more complex trees in the same scene/environment, but we encountered some issues. To create the water, I needed to use Unity’s Universal Render Pipeline (URP) in order to create a new shader graph. However, when switching over from the Built-In Render Pipeline that my partner used when creating the trees and terrain, we noticed that the trees had been transformed from their original, proper color to just solid pink masses. Upon closer inspection, we found that the shaders used for the trees – Nature/Tree Creator – are not supported with URP. We tried to fix this by switching them over to the URP/Nature/SpeedTree7 and SpeedTree8 shaders. While it did improve things a bit, adding back some proper colors to the trees rather than just the solid pink they were erroneously changed to, it still did not revert them back to the original, proper-looking trees from the Built-In Render Pipeline. We could not figure out how to keep both the water created using URP and the trees we made using the Built-In Render Pipeline, so ultimately we decided to create two separate scenes and executables: one showcasing the water and the other showing the complex trees.
