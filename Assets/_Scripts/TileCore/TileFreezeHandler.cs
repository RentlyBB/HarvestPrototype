using System;
using _Scripts.TileCore.Enums;
using UnityEngine;
using VInspector;

namespace _Scripts.TileCore {
    [RequireComponent(typeof(TileVisualHandler))]
    public sealed class TileFreezeHandler : MonoBehaviour {
        
        private TileVisualHandler _tileVisualHandler;

        private TileMainVisualStates _originalVisualMainState;

        private void Awake() {
            TryGetComponent(out _tileVisualHandler);
        }

        [Button]
        public void FreezeTile() {
            if (_tileVisualHandler) {
                // Safe the last visual state before freezing
                _originalVisualMainState = _tileVisualHandler.CurrentMainState;
                _tileVisualHandler.SetMainState(TileMainVisualStates.FreezeState);
                
            } else {
                Debug.LogError("TileVisualHandler is missing on this tile!");
            }
        }

        [Button]
        public void UnfreezeTile() {
            if (_tileVisualHandler != null) {
                _tileVisualHandler.SetMainState(_originalVisualMainState);
            }else {
                Debug.LogError("TileVisualHandler is missing on this tile!");
            }
        }
    }
}