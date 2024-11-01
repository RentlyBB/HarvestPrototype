using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.SOs {
    [CreateAssetMenu(fileName = "GridLevelData", menuName = "Custom/GridLevelData", order = 0)]
    public class GridLevelData_old : ScriptableObject {

        public int levelID;
        public int goal;
        
        public Vector2Int gridSize;
        public Vector2Int playerStartingPosition;

        /// <summary>
        /// X - Not moveable
        /// M - Moveable
        /// N5 - Value
        /// </summary>
        public List<string> tileData = new List<string>();
    }
}