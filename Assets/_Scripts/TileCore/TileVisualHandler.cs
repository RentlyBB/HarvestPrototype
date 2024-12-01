using System;
using _Scripts.TileCore.Enums;
using _Scripts.TileCore.ScriptableObjects;
using UnityEngine;
using VInspector;

namespace _Scripts.TileCore {
    [RequireComponent(typeof(SpriteRenderer))]
    public class TileVisualHandler : MonoBehaviour {

        public TileVisualData tileVisualData; // Assign this in the Inspector

        public TileMainVisualStates CurrentMainState { get; private set;}
        public TileSubVisualStates CurrentSubState { get; private set; }


        private SpriteRenderer _spriteRenderer;


        private Color _originalColor;
        
        private void Awake() {
            CurrentSubState = TileSubVisualStates.Unpressed;
            
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
            Sprite sprite = tileVisualData.GetSprite(CurrentMainState, CurrentSubState);
            _spriteRenderer.sprite = sprite;
            
        }

        private void HandleTransparency() {
            if (CurrentMainState is TileMainVisualStates.Empty) {
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
            CurrentMainState = newMainState;
            CurrentSubState = newSubState;
            UpdateSprite();
        }

        public void SetMainState(TileMainVisualStates newMainState) {
            CurrentMainState = newMainState;
            UpdateSprite();
        }

        public void SetSubState(TileSubVisualStates newSubState) {
            if (CurrentMainState is TileMainVisualStates.Empty) return;

            CurrentSubState = newSubState;
            UpdateSprite();
        }

        public void SetColorOver() {
            _spriteRenderer.color = Color.cyan;
        }

        public void ResetColorOver() {
            _spriteRenderer.color = _originalColor;
        }
    }
}