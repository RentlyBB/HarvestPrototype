using System;
using _Scripts.Enums;
using _Scripts.TileCore.Enums;
using EditorScripts;
using UnityEngine;

namespace _Scripts.TileCore {
    [RequireComponent(typeof(TileVisualHandler))]
    public class TileStateHandler : MonoBehaviour {
        public TileState currentState;

        private TileVisualHandler _tileVisualHandler;

        private void Awake() {
            TryGetComponent(out _tileVisualHandler);
        }

        public void ChangeState(TileState state) {
            currentState = state;
            _tileVisualHandler.UpdateSprite(currentState);
        }

        [InvokeButton]
        public void SwitchState() {
            switch (currentState) {
                case TileState.Normal:
                    ChangeState(TileState.Freeze);
                    break;
                case TileState.Freeze:
                    ChangeState(TileState.Normal);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _tileVisualHandler.UpdateSprite(currentState);
        }
    }
}