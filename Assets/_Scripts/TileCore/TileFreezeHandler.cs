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

        private TileMainVisualStates _originalVisualMainState;
        
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
            //TODO: Predelat last visual state, dela to bug v prechodu z not ready to collect na ready to collect
            _originalVisualMainState = _tileVisualHandler.CurrentMainState;
            _tileVisualHandler?.QueueVisualChange(TileMainVisualStates.FreezeState, null);
        }


        [Button]
        public void UnfreezeTile() {
            _tileVisualHandler?.QueueVisualChange(_originalVisualMainState, null);
        }
    }
}