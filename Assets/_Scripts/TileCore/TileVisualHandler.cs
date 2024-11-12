using System;
using _Scripts.TileCore.Enums;
using _Scripts.TileCore.ScriptableObjects;
using UnityEngine;

namespace _Scripts.TileCore {
    [RequireComponent(typeof(SpriteRenderer))]
    public class TileVisualHandler : MonoBehaviour {

        public TileVisualData tileVisualData; // Assign this in the Inspector

        private SpriteRenderer _spriteRenderer;
        public TileMainVisualStates _currentMainState;
        public TileSubVisualStates _currentSubState = TileSubVisualStates.Unpressed;

        private void Awake() {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void UpdateSprite() {
            if (_currentMainState is TileMainVisualStates.Empty) {
                _spriteRenderer.color = new Color(0, 0, 0, 0);
                return;
            }

            if (tileVisualData is null) {
                Debug.LogError("TileVisualData is not assigned!");
                return;
            }

            // Retrieve and apply the appropriate sprite for the current composite state
            Sprite sprite = tileVisualData.GetSprite(_currentMainState, _currentSubState);
            _spriteRenderer.sprite = sprite;
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
            if (_currentMainState is TileMainVisualStates.Empty) return;

            _currentSubState = newSubState;
            UpdateSprite();
        }
    }
}