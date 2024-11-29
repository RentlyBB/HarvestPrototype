using _Scripts.Managers;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using UnityEngine;
using VInspector;

namespace _Scripts.TileCore.Tiles {
    public class FreezeVerticalTile : TileBase {
        
        private void Start() {
            tileVisualHandler.SetMainAndSubState(TileMainVisualStates.DefaultState, TileSubVisualStates.Unpressed);
        }

        private void FreezeLine() {
            var grid = GridManager.Instance.GetGrid();

            for (int i = 0; i < grid.GetHeight(); i++) {
                //grid.GetGridObject(new Vector2(gridPosition.x, i)).GetTile().tileVisualHandler.SetColorOver();
            }
        }
    }
}