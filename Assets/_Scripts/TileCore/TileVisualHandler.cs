using System;
using _Scripts.CustomTools;
using _Scripts.TileCore.Enums;
using UnityEngine;

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

        public void UpdateSprite(TileState tileState) {
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