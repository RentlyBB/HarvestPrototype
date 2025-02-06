using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.TileCore.Enums;
using _Scripts.TileCore.ScriptableObjects;
using UnityEngine;
using VInspector;

namespace _Scripts.TileCore {
    [RequireComponent(typeof(SpriteRenderer))]
    public class TileVisualHandler : MonoBehaviour {
        public TileVisualData tileVisualData; // Assign this in the Inspector
        public TileMainVisualStates CurrentMainState { get; private set; }
        public TileSubVisualStates CurrentSubState { get; private set; }

        private SpriteRenderer _spriteRenderer;

        private void Awake() {
            CurrentSubState = TileSubVisualStates.Unpressed;
            TryGetComponent(out _spriteRenderer);
        }

        private void UpdateSprite() {
            if (tileVisualData is null) {
                Debug.LogError("TileVisualData is not assigned!");
                return;
            }
            
            if(!_spriteRenderer) return;

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

        /// <summary>
        /// Add a visual change task to the queue. Specify null for states you don't want to change.
        /// </summary>
        /// <param name="newMainState">The new main state to set, or null to keep the current state.</param>
        /// <param name="newSubState">The new sub state to set, or null to keep the current state.</param>
        /// <param name="delay">Optional delay before applying this change.</param>
        public void ProcessVisualChange(TileMainVisualStates? newMainState, TileSubVisualStates? newSubState) {
            // Apply the visual change (only change states that are not null)
            if (newMainState.HasValue) {
                CurrentMainState = newMainState.Value;
            }

            if (newSubState.HasValue) {
                CurrentSubState = newSubState.Value;
            }

            UpdateSprite();
        }
    }
}