using System;
using _Scripts.PlayerCore;
using _Scripts.TileCore.Enums;
using _Scripts.TileCore.ScriptableObjects;
using UnityEngine;

namespace _Scripts.TileCore {
    [RequireComponent(typeof(SpriteRenderer))]
    public class TileVisualHandler : MonoBehaviour {
        
        public TileVisualData tileVisualData; // Assign this in the Inspector

        private SpriteRenderer _spriteRenderer;
        private TileMainVisualStates _currentMainState;
        private TileSubVisualStates _currentSubState;

        private void Awake() {
            _spriteRenderer = GetComponent<SpriteRenderer>();

        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void UpdateSprite() {
            if (tileVisualData == null) {
                Debug.LogError("TileVisualData is not assigned!");
                return;
            }

            // Retrieve and apply the appropriate sprite for the current composite state
            Sprite sprite = tileVisualData.GetSprite(_currentMainState, _currentSubState);
            if (sprite is not null) {
                _spriteRenderer.sprite = sprite;
            }
        }

        public void SetMainAndSubState(TileMainVisualStates newMainState, TileSubVisualStates newSubState) {
            _currentMainState = newMainState;
            _currentSubState = newSubState;
            UpdateSprite();
        }

        public void SetMainState(TileMainVisualStates newMainState) {
            _currentMainState = newMainState;
            UpdateSprite();
        }

        public void SetSubState(TileSubVisualStates newSubState) {
            _currentSubState = newSubState;
            UpdateSprite();
        }
        
        
    }
}