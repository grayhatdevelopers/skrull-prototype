# Skrull 

A simple card game built with [Unity](https://unity.com/) and [Playroomkit](https://github.com/playroomkit/Playroomkit). This project is inspired by ideas detailed in the [Skrull GDD](https://gifted-punch-51c.notion.site/Skrull-GDD-1812d76eeeaa808aaca5fff530310cd9?pvs=74).
- Project made with Unity `6000.0.25f1`

> **Game Link:**  
https://skurll.playroom.gg

## Overview

**Skrull** is designed as a straightforward card game experience implemented in Unity. It leverages Playroomkit for multiplayer. This repository contains all the source code, assets, and project settings needed to run and modify the game.

## How to Use this Repository

1. **Clone the Repository**
    ```bash
    git clone https://github.com/grayhatdevelopers/skrull-prototype.git
    ```

2. **Open the Project in Unity**
    - Launch [Unity Hub](https://unity.com/download).
    - Click on **Add** and navigate to the cloned repository’s folder.
    - **Note:** Project is made with Unity `6000.0.27f1`
    - Open the project using the Unity version specified in the repository (refer to the project settings if necessary).

3. **Install Dependencies**
    - Ensure you have [Playroomkit](https://github.com/playroomkit/unity) installed by following their documentation.
    - Project also uses [UnityHFSM](https://github.com/Inspiaaa/UnityHFSM) but it is optional, you can use any way to handle game flow.

4. **Run the Game**
    - TODO: Open the main scene (typically found in the `Scenes` folder).
    - Click the **Play** button in the Unity Editor to test the game.
    - Use Unity’s Build Settings to compile the project for your target platform.

## Code Documentation

The repository is organized as follows:

- **Assets/**  
  Contains all game assets, scripts, and scenes.
  
  - **Scripts/**  
    Contains the game logic and gameplay scripts. Each script includes inline comments documenting the code functionality.
  
  - **Scenes/**  
    Contains the Unity scenes with the main gameplay usually located in `MainScene.unity`.

## Need more?
Getting started with [Playroom Unity](https://docs.joinplayroom.com/usage/unity).
Playroom Unity API ref: https://docs.joinplayroom.com/apidocs/unity

Happy Jaming!
