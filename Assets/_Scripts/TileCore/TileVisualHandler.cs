using System;
using _Scripts.CustomTools;
using _Scripts.Enums;
using UnityEngine;

namespace _Scripts.TileCore {
    [RequireComponent(typeof(SpriteRenderer), typeof(TileStateHandler))]
    public class TileVisualHandler : MonoBehaviour {
        
        [RequireVariable]
        public Sprite baseSprite, freezeSprite;
        
        private SpriteRenderer _spriteRenderer;
        private TileStateHandler _tileStateHandler;

        private void Awake() {
            TryGetComponent(out _spriteRenderer);
            TryGetComponent(out _tileStateHandler);
            _spriteRenderer.sprite = baseSprite;
        }
        
        public void ChangeSprite() {
            switch (_tileStateHandler.tileState) {
                case TileState.Normal:
                    _spriteRenderer.sprite = baseSprite;
                    break;
                case TileState.Freeze:
                    _spriteRenderer.sprite = freezeSprite;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_tileStateHandler.tileState), _tileStateHandler.tileState, null);
            }
        }
    }
}