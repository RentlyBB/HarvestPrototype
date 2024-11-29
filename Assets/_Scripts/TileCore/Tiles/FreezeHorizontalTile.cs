using System;
using System.Collections.Generic;
using _Scripts.Managers;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using UnityEngine;
using VInspector;

namespace _Scripts.TileCore.Tiles {
    public class FreezeHorizontalTile : TileBase {

        private void Start() {
            tileVisualHandler.SetMainAndSubState(TileMainVisualStates.DefaultState, TileSubVisualStates.Unpressed);
        }

        private void FreezeLine() {
            var grid = GridManager.Instance.GetGrid();

            for (int i = 0; i < grid.GetWidth(); i++) {
                //grid.GetGridObject(new Vector2(i, gridPosition.y)).GetTile().tileVisualHandler.SetColorOver();
            }
        }
    }
}