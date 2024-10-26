using System.Collections.Generic;
using _Scripts.GameplayCore.Tiles;
using UnityEngine;

namespace _Scripts.GridCore {
    public class LevelDataObj {
        public int width, height;
        public Vector2 playerStartingPosition;
        
        public List<TileCore> tiles = new List<TileCore>();
    }
}