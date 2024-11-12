using System;
using _Scripts.TileCore;
using _Scripts.TileCore.Enums;
using UnityEngine;

namespace _Scripts.LevelEditor {
    [RequireComponent(typeof(TileVisualHandler), typeof(TileTextHandler))]
    public class EditorTile : MonoBehaviour {

        public TileVisualHandler tileVisualHandler;
        public TileTextHandler tileTextHandler;

        public int countdownValue;
        
        public TileType tileType;
        
        private void Awake() {
            TryGetComponent(out tileVisualHandler);
            TryGetComponent(out tileTextHandler);
        }

        private void Start() {
            tileVisualHandler.SetSubState(TileSubVisualStates.Unpressed);
        }
    }
}