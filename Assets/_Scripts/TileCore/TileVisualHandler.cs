using System;
using _Scripts.CustomTools;
using UnityEngine;
using _Scripts.Enums;
using UnityEngine.Serialization;

namespace _Scripts.TileCore {
    [RequireComponent(typeof(SpriteRenderer))]
    public class TileVisualHandler : MonoBehaviour {
        
        [RequireVariable]
        public Sprite baseSprite, freezeSprite;

        private SpriteRenderer _spriteRenderer;

        private void Awake() {
            TryGetComponent(out _spriteRenderer);
            _spriteRenderer.sprite = baseSprite;
        }

        private void OnEnable() {
            TileStateHandler.OnStateChanged += ChangeSprite;
        }

        private void OnDisable() {
            TileStateHandler.OnStateChanged -= ChangeSprite;
        }

        private void ChangeSprite(TileState tileState) {
            switch (tileState) {
                case TileState.Normal:
                    _spriteRenderer.sprite = baseSprite;
                    break;
                case TileState.Freeze:
                    _spriteRenderer.sprite = freezeSprite;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tileState), tileState, null);
            }
        }
    }
}