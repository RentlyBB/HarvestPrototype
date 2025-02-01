using System;
using _Scripts.Managers;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using UnityEngine;
using VInspector;

namespace _Scripts.TileCore {
    [RequireComponent(typeof(TileVisualHandler))]
    public sealed class TileFreezeHandler : MonoBehaviour {
        
        private TileVisualHandler _tileVisualHandler;

        [ShowInInspector]private TileMainVisualStates _originalVisualMainState;
        
        private void Awake() {
            TryGetComponent(out _tileVisualHandler);
        }
        
        [Button]
        public void FreezeTile() {
            TryGetComponent(out CountdownTileBase countdownTileBase);
            countdownTileBase?.SkipNextDecrease();
        }

        public void FreezeVisual() {
            // Safe the last visual state before freezing
            _originalVisualMainState = _tileVisualHandler.CurrentMainState;
            _tileVisualHandler?.ProcessVisualChange(TileMainVisualStates.FreezeState, null);
        }


        [Button]
        public void UnfreezeTile() {
            _tileVisualHandler?.ProcessVisualChange(_originalVisualMainState, null);
        }
    }
}