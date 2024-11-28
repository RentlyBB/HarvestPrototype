using System;
using System.Collections.Generic;
using _Scripts.Managers;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using UnityEngine;
using VInspector;

namespace _Scripts.TileCore.Tiles {
    public class FreezeTile : TileBase {

        public bool horizontalFreeze = true;

        private void Start() {
            var visualState = horizontalFreeze ? TileMainVisualStates.FreezeHorizontal : TileMainVisualStates.FreezeVertical;
            tileVisualHandler.SetMainAndSubState(visualState, TileSubVisualStates.Unpressed);

        }

        private void FreezeLine() {
        }

        [Button]
        public void FreezeRow() {

            var grid = GridManager.Instance.GetGrid();

            for (int i = 0; i < grid.GetWidth(); i++) {
                //grid.GetGridObject(new Vector2(i, gridPosition.y)).GetTile().tileVisualHandler.SetColorOver();
            }
        }

        [Button]
        public void FreezeColumn() {
            var grid = GridManager.Instance.GetGrid();

            for (int i = 0; i < grid.GetHeight(); i++) {
                //grid.GetGridObject(new Vector2(gridPosition.x, i)).GetTile().tileVisualHandler.SetColorOver();
            }
        }
    }
}