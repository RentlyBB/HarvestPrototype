using _Scripts.Managers;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using UnityEngine;

namespace _Scripts.TileCore.Tiles {
    public class FreezeVerticalTile : TileBase {
        private void Start() {
            tileVisualHandler.SetMainAndSubState(TileMainVisualStates.DefaultState, TileSubVisualStates.Unpressed);
        }

        public override void OnPlayerStep() {
            base.OnPlayerStep();
            FreezeLine();
        }

        private void FreezeLine() {
            var grid = GridManager.Instance.GetGrid();


            for (var i = 0; i < grid.GetHeight(); i++) {
                var tile = grid.GetGridObject(new Vector2(gridPosition.x, i)).GetTile();

                if (tile.TryGetComponent(out TileFreezeHandler tileFreezeHandler)) {
                    tileFreezeHandler.FreezeTile();
                }
            }
        }
    }
}