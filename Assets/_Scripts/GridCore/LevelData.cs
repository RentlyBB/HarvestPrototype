using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.GridCore {
    [CreateAssetMenu(fileName = "LevelData", menuName = "Custom/LevelData", order = 0)]
    public class LevelData : ScriptableObject {

        public int width, height;
        public Vector2 playerStartingPosition;
        public List<TileGridObject> Tiles = new List<TileGridObject>();

    }
}