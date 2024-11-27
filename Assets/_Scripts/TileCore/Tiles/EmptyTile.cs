using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using UnityEngine;

namespace _Scripts.TileCore.Tiles {
    public class EmptyTile : TileBase {

        private void Start() {
            tileVisualHandler.SetMainState(TileMainVisualStates.Empty);
            canMoveOn = false;
        }
    }
}