using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using UnityEngine;

namespace _Scripts.TileCore.Tiles {
    public class EmptyTile : TileBase {

        public override void SetupTile() {
            tileVisualHandler.QueueVisualChange(TileMainVisualStates.Empty, null);
            canMoveOn = false;
        }
    }
}