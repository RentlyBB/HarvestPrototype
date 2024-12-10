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

      
        private void OnDisable() {
            GameplayManager.UnfreezeTiles -= UnfreezeTile;
        }

        private void Awake() {
            TryGetComponent(out _tileVisualHandler);
        }

        [Button]
        public void FreezeTile() {
            
            GameplayManager.UnfreezeTiles += UnfreezeTile;
        }

        public void FreezeVisual() {
            // Safe the last visual state before freezing
            _originalVisualMainState = _tileVisualHandler.CurrentMainState;
            _tileVisualHandler?.QueueVisualChange(TileMainVisualStates.FreezeState, null);
        }


        [Button]
        private void UnfreezeTile() {
            
            TryGetComponent(out CountdownTileBase countdownTileBase);
            countdownTileBase?.SkipNextDecrease();
            
            _tileVisualHandler?.QueueVisualChange(_originalVisualMainState, null);
            
            GameplayManager.UnfreezeTiles -= UnfreezeTile;
        }
    }
}