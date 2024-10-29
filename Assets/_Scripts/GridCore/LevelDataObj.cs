using System.Collections.Generic;
using _Scripts.TileCore;
using _Scripts.TileCore.BaseClasses;
using UnityEngine;

namespace _Scripts.GridCore {
    public class LevelDataObj {
        public int width, height;
        public Vector2 playerStartingPosition;
        
        public List<TileBase> tiles = new List<TileBase>();
    }
}