# Live-Ops Game Skeleton

This project is a Unity prototype that demonstrates how to handle **live-ops / active events** in a game.  
It includes a simple main menu, a mock backend, asset loading, and event display logic.

---

## Features
- **Main Menu**
  - Displays the current player level.
  - Includes a **Play** button (each press increases the player level by 1).

- **Live-Ops / Events**
  - Events are requested from a **mock server** (fully configurable).
  - At least three different types of events are included.
  - Events are filtered by **required player level** (too-low level = hidden).
  - Active playable events are shown in the menu with a **countdown to expiration**.
  - Expired events remain visible until clicked by the player.

- **Event Popups**
  - Each event has its own popup with themed assets (e.g. Winter, Summer, etc.).
  - Assets (active icon, expired icon, popup UI) are **loaded as a group** via Unity Addressables.
  - Clicking an event shows its popup and increments a **click counter**.
  - The click counter is **persisted across sessions** and reset on the next repetition of the event.

---

## Configuration
Event data is controlled through a **ScriptableObject** located at: Assets/AddressableAssets/Settings/LOEventsMockData

You can edit:
- `uniqueId`  
- `name`  
- `requiredLevel`  
- `endTimestampInSeconds`  
- `addressableLabel`  
- `keysToAssets` (assets to load for the event)

---

## How to Use
1. Open the project in Unity.  
2. Switch scene to MainScene
3. Enter Play Mode:
   - See your current **player level** in the main menu.
   - Press **Play** to increase your level.
   - Watch events appear/disappear depending on level and expiration.
4. Click on an event to:
   - Open its popup with themed assets.
   - Increase and persist its click counter.  
   - Dismiss expired events once interacted with.

---

## Notes
- Asset theming is mocked (no need for final art/animations).  
- Built with **Unity Addressables** for scalable asset handling.
- All core managers inherit from interfaces just to make testing/replacing with other types faster.