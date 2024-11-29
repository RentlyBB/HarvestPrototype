using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.TileCore.ScriptableObjects;
using UnityEngine;
using VInspector;

namespace _Scripts.GameplayCore {
    [CreateAssetMenu(fileName = "Level", menuName = "Level/New Level", order = 0)]
    public class LevelData : ScriptableObject {
        [ShowInInspector]public int GridWidth { get; private set; }
        [ShowInInspector]public int GridHeight { get; private set; }
        
        public Vector2Int startingGridPosition;
        public List<TileData> tiles = new List<TileData>();

        [Button]
        private void UpdateTilesList() {
            int totalTiles = GridWidth * GridHeight;

            if (totalTiles <= 0) {
                tiles.Clear();
                return;
            }
            
            var tempTiles = new List<TileData>(totalTiles);

            // Initialize tempTiles with default TileData
            for (int y = 0; y < GridHeight; y++) {
                for (int x = 0; x < GridWidth; x++) {
                    var gridPosition = new Vector2Int(x, y);

                    // Try to find an existing tile at this position
                    var existingTile = tiles.FirstOrDefault(tile => tile.gridPosition == gridPosition);

                    if (existingTile != null) {
                        // If an existing tile matches, keep it
                        tempTiles.Add(existingTile);
                    } else {
                        // Otherwise, create a new default tile
                        var newTile = new TileData(Resources.Load<TileTypeData>("DefaultTileTypes/EmptyTile")) {
                            gridPosition = gridPosition,
                            countdownValue = 0
                        };
                        tempTiles.Add(newTile);
                    }
                }
            }

            // Replace the old tiles list with the updated list
            tiles = tempTiles;
        }
        
        public void SetGridSize(int width, int height) {
            GridWidth = width;
            GridHeight = height;
            UpdateTilesList();
        }
    }

    [Serializable]
    public class TileData {
        public TileData(TileTypeData tileTypeData) {
            this.tileTypeData = tileTypeData;
        }
        
        public Vector2Int gridPosition;
        public TileTypeData tileTypeData;
        public int countdownValue = 0;
    }
}