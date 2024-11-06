using System;
using _Scripts.TileCore.Enums;
using UnityEngine;

namespace _Scripts.TileCore {
    public class TileStateHandler : MonoBehaviour {
        public TileState currentState;

        public void ChangeState(TileState state) {
            currentState = state;
            
        }
    }
}