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
    }
    
    [System.Serializable]
    public class TileData {
        public Vector2Int gridPosition;
        public TileType tileType; // Define an enum or class for tile types
        public int countdownValue = 0;
    }
}