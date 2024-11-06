using System;
using _Scripts.PlayerCore;
using _Scripts.TileCore.Enums;
using UnityEngine;

namespace _Scripts.TileCore {
    [RequireComponent(typeof(SpriteRenderer), typeof(TileStateHandler))]
    public class TileVisualHandler : MonoBehaviour {
        
        public Sprite unpressedSprite;
        public Sprite pressedSprite;

        private SpriteRenderer _spriteRenderer;
        private TileStateHandler _tileStateHandler;
        private TileVisualState _tileVisualState;

        private void Awake() {
            TryGetComponent(out _spriteRenderer);
            TryGetComponent(out _tileStateHandler);
            
            _spriteRenderer.sprite = unpressedSprite;
            _tileVisualState = TileVisualState.Unpressed;
        }

        public void UpdateSprite() {
            switch (_tileStateHandler.currentState) {
                case TileState.Normal:
                    _spriteRenderer.sprite = _tileVisualState == TileVisualState.Unpressed ? unpressedSprite : pressedSprite;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_tileStateHandler.currentState), _tileStateHandler.currentState, null);
            }
        }

        public void ChangeVisualState(TileVisualState tileVisualState) {
            _tileVisualState = tileVisualState;
            
            UpdateSprite();
        }
        
        
    }
}