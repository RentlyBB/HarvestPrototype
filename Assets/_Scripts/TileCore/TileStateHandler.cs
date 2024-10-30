using System;
using _Scripts.Enums;
using EditorScripts;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace _Scripts.TileCore {
    public class TileStateHandler : MonoBehaviour {
        
        public TileState tileState;

        public void ChangeState(TileState state) {
            tileState = state;
        }

        [InvokeButton]
        public void SwitchState() {
            switch (tileState) {
                case TileState.Normal:
                    tileState = TileState.Freeze;
                    break;
                case TileState.Freeze:
                    tileState = TileState.Normal;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}