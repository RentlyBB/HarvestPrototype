using System;
using _Scripts.TileCore.Enums;
using _Scripts.TileCore.ScriptableObjects;
using UnityEngine;
using VInspector;

namespace _Scripts.TileCore {
    [RequireComponent(typeof(SpriteRenderer))]
    public class TileVisualHandler : MonoBehaviour {

        public TileVisualData tileVisualData; // Assign this in the Inspector

        public TileMainVisualStates _currentMainState;
        public TileSubVisualStates _currentSubState = TileSubVisualStates.Unpressed;

        
        private SpriteRenderer _spriteRenderer;


        private Color _originalColor;
        
        private void Awake() {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _originalColor = _spriteRenderer.color;
        }

        private void UpdateSprite() {
            if (tileVisualData is null) {
                Debug.LogError("TileVisualData is not assigned!");
                return;
            }

            HandleTransparency();

            // Retrieve and apply the appropriate sprite for the current composite state
            Sprite sprite = tileVisualData.GetSprite(_currentMainState, _currentSubState);
            _spriteRenderer.sprite = sprite;
            
        }

        private void HandleTransparency() {
            if (_currentMainState is TileMainVisualStates.Empty) {
                Color col = _spriteRenderer.color;
                col.a = 0;
                _spriteRenderer.color = col;
                return;
            }

            if (_spriteRenderer.color.a == 0) {
                Color color = _spriteRenderer.color;
                color.a = 255;
                _spriteRenderer.color = color;
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
            if (_currentMainState is TileMainVisualStates.Empty) return;

            _currentSubState = newSubState;
            UpdateSprite();
        }
    }
}