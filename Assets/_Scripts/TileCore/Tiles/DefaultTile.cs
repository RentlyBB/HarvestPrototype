using System;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using UnityEngine;

namespace _Scripts.TileCore.Tiles {
    public class DefaultTile : TileBase {
        
        public override void SetupTile() {
            tileVisualHandler.QueueVisualChange(TileMainVisualStates.DefaultState, null);
        }
    }
}