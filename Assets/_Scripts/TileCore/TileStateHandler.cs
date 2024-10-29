using System;
using _Scripts.Enums;
using EditorScripts;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.TileCore {
    public class TileStateHandler : MonoBehaviour {
        
        public static event UnityAction<TileState> OnStateChanged = delegate { };

        private TileState _tileState;

        public void ChangeState(TileState state) {
            _tileState = state;
            OnStateChanged?.Invoke(_tileState);
        }

        [InvokeButton]
        public void SwitchState() {
            switch (_tileState) {
                case TileState.Normal:
                    _tileState = TileState.Freeze;
                    break;
                case TileState.Freeze:
                    _tileState = TileState.Normal;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            OnStateChanged?.Invoke(_tileState);
        }
    }
}