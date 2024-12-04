using System;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using UnityEngine;
using UnityEngine.Serialization;
using VInspector;

namespace _Scripts.TileCore.ScriptableObjects {
    [CreateAssetMenu(fileName = "TileTypeData", menuName = "Tiles/TileTypeData", order = 0)]
    public class TileTypeData : ScriptableObject {
        [TextArea]
        public string description;
        public GameObject tilePrefab;
        public TileMainVisualStates mainVisualStates;
        [ShowIf("_isCountdownTile")]
        public int defaultCountdownValue;
        
        private bool _isCountdownTile = false;
        
        private void OnValidate() {
            _isCountdownTile = tilePrefab?.GetComponent<CountdownTileBase>();
        }
        
    }
}