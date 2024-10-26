using System;
using System.Collections.Generic;
using _Scripts.GameplayCore.Tiles;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.GridCore {
    [CreateAssetMenu(fileName = "LevelData", menuName = "Custom/LevelData", order = 0)]
    public class LevelDataSo : ScriptableObject {

        public int width, height;
        public Vector2 playerStartingPosition;
        
        public List<TileCore> tiles = new List<TileCore>();
    }
}