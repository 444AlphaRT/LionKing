# Simba's Survival

A small **top–down survival game** built in Unity for the course assignment.

You play as **young Simba** exploring the savanna, searching for food and trying to survive as long as possible, while managing **health** and **hunger** and avoiding death by starvation.

Link to play in itch.io: 
---

## 🎮 Core Gameplay

- You control **Simba** in a close-up top–down view.
- The **camera follows** Simba as he moves around the world.
- Simba has:
  - A **hearts bar** (health)
  - A **hunger bar** (food)
- **Food spawns randomly** around the map in the form of:
  - 🍗 **Chicken** – common, restores a small amount of hunger
  - 🥩 **Steak** – very rare, restores a larger amount of hunger
- Food items appear at **random positions in the world** and exist only for a **limited time** before disappearing.
- If the **hunger bar reaches zero**, Simba starts to **lose hearts over time** (starvation).
- Simba can also **regenerate health slowly** by consuming food:
  - Health goes up **by one heart**
  - Hunger goes down **by a configurable amount of units**
- If Simba loses all hearts → **Game Over** screen appears with:
  - Full-screen background illustration
  - Crying Simba in the corner
  - A **Restart** button to restart the level
- A **survival timer** runs from the start of the game and shows how long Simba has survived.

---

## 🧠 Main Game Systems

### Player Movement & Camera
- **PlayerController**:
  - Handles keyboard input and movement of Simba.
  - Uses Unity’s `Rigidbody2D` for smooth movement.
- **Camera Follow**:
  - The camera tracks Simba so he is always centered in view.
- **World Bounds**:
  - Invisible colliders around the map prevent Simba from leaving the playable area.

### Health & Hunger (PlayerStats)
The `PlayerStats` component is responsible for **all player stats**:

- **Hearts (Health)**:
  - `maxHearts` and `currentHearts`.
  - Hearts can be reduced by starvation (and optionally by enemies in the future).
- **Food (Hunger)**:
  - `maxFoodUnits` and `currentFoodUnits`.
  - Food is reduced **over time** (configurable in the inspector).
- **Starvation**:
  - If `currentFoodUnits` reaches zero, a starvation timer starts.
  - Every starvation tick reduces Simba’s hearts.
- **Health Regeneration from Food**:
  - Optional system: if enabled, Simba can slowly regenerate hearts using food.
  - Every regeneration step:
    - Heals **one heart**
    - Consumes a configurable number of food units
- **Statistics**:
  - `totalFoodCollected` – how much food Simba has collected during the run.
  - `survivalTimeSeconds` is tracked in the `GameManager` (see below).

All timing and amounts are implemented with **serialized fields**, so there are **no magic numbers** inside the code logic. Designers can tweak everything from the inspector.

### Food & Spawning

There are **two types of food** implemented as prefabs:

- **FoodCommon** (chicken)
  - Restores a small amount of hunger.
  - Common spawn.
- **FoodRare** (steak)
  - Restores a larger amount of hunger.
  - Has a very low spawn chance.

Each food prefab has:

- `SpriteRenderer` – shows the art.
- `Collider2D` with `IsTrigger` enabled – to detect when Simba overlaps it.
- `FoodCollectible` script:
  - On trigger with a `Player` object:
    - Calls `PlayerStats.AddFoodUnits(amount)`
    - Destroys the food object
- `FoodLifetime` script:
  - Each food item gets a **random lifetime** between `minLifetimeSeconds` and `maxLifetimeSeconds`.
  - After that time, the food object is automatically destroyed if not eaten.

#### FoodSpawner

The `FoodSpawner` is responsible for **spawning food randomly on the map**:

- Uses two points in the world:
  - `minSpawnPoint` – bottom-left corner of the spawn area
  - `maxSpawnPoint` – top-right corner of the spawn area
- Chooses random `(x, y)` positions inside this rectangle.
- Spawns waves of food over time:
  - `foodItemsPerSpawn` – how many pieces of food per wave
  - `minSpawnIntervalSeconds`, `maxSpawnIntervalSeconds` – random time between waves
  - `rareFoodChance` – probability that a spawned item will be the rare steak

Again, all parameters are **tunable from the inspector**, so designers can adjust difficulty and pacing without changing code.

---

## 💔 Game Over & Restart

**GameManager** handles:

- Monitoring `PlayerStats.IsDead()`.
- When Simba dies:
  - Freezes time using `Time.timeScale = gameOverTimeScale`.
  - Disables player movement (via the player controller).
  - Activates the **Game Over UI panel**.
- The Game Over panel includes:
  - A full-screen background image
  - Crying Simba illustration
  - A custom **Restart** button (image-based)
- The Restart button calls `GameManager.RestartLevel()`:
  - Restores `Time.timeScale` to `normalTimeScale`
  - Reloads the active scene

---

## ⏱️ Survival Timer UI

The game tracks **how long the player survived**:

- `GameManager` keeps `survivalTimeSeconds` and updates it every frame while the player is alive.
- `SurvivalTimerUI`:
  - Reads the time from `GameManager`.
  - Formats it as `MM:SS`.
  - Updates a TextMeshProUGUI element on the HUD (for example: `Time: 03:27`).

---

## ❤️ HUD (UI)

The HUD uses Unity UI and TextMeshPro and includes:

- **Hearts bar**:
  - Shows current health vs. max hearts.
  - Uses full/empty heart sprites.
- **Hunger bar**:
  - Shows current food units.
  - Uses full/empty food icons.
- **Survival timer**:
  - Text in a corner of the screen, always visible during gameplay.

All UI scripts follow the rule:

> They **only read data from** `PlayerStats` / `GameManager` and **never change gameplay logic**.

---

## 🧱 Project Structure (Scripts)

Example structure:

```text
Assets/
  Scripts/
    Player/
      PlayerController.cs
      PlayerStats.cs
    Managers/
      GameManager.cs
      FoodSpawner.cs
    Collectibles/
      FoodCollectible.cs
      FoodLifetime.cs
    UI/
      HeartsUI.cs
      HungerUI.cs
      SurvivalTimerUI.cs