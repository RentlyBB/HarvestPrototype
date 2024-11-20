using System;
using System.Collections.Generic;
using _Scripts.TileCore.Enums;
using UnityEngine;

namespace _Scripts.GameplayCore {
    [CreateAssetMenu(fileName = "New Level", menuName = "Custom/New Level", order = 0)]
    public class LevelData : ScriptableObject {
        public int gridWidth;
        public int gridHeight;
        public Vector2Int startingGridPosition;
        public List<TileData> tiles = new List<TileData>();
        
        public void UpdateTilesList() {
            int totalTiles = gridWidth * gridHeight;

            if (totalTiles <= 0) {
                tiles.Clear();
                return;
            }

            // Ensure the tiles list has exactly the right number of elements
            while (tiles.Count < totalTiles) {
                tiles.Add(new TileData());
            }

            while (tiles.Count > totalTiles) {
                tiles.RemoveAt(tiles.Count - 1);
            }

            // Assign default grid positions for each tile
            for (int y = 0; y < gridHeight; y++) {
                for (int x = 0; x < gridWidth; x++) {
                    int index = y * gridWidth + x;
                    tiles[index].gridPosition = new Vector2Int(x, y);
                    tiles[index].tileType = TileType.DefaultTile; // Set a default TileType if needed
                    tiles[index].countdownValue = 0; // Default value
                }
            }
        }
    }
    
    [Serializable]
    public class TileData {
        public Vector2Int gridPosition;
        public TileType tileType; // Define an enum or class for tile types
        public int countdownValue = 0;
    }
}