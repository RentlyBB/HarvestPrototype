using System.Collections;
using System.Threading.Tasks;
using _Scripts.Managers;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using UnityEngine;

namespace _Scripts.TileCore.Tiles {
    public class FreezeHorizontalTile : TileBase {

        public override void SetupTile() {
            tileVisualHandler.QueueVisualChange(TileMainVisualStates.DefaultState, TileSubVisualStates.Unpressed);
        }

        public override async Task OnPlayerStep() {
            await base.OnPlayerStep();
            await FreezeLine();
        }

        private async Task FreezeLine() {
            var grid = GridManager.Instance.GetGrid();

            // Freeze Tile
            for (var i = 0; i < grid.GetWidth(); i++) {
                var tile = grid.GetGridObject(new Vector2(i, gridPosition.y)).GetTile();
                if (tile.TryGetComponent(out TileFreezeHandler tileFreezeHandler)) {
                    tileFreezeHandler.FreezeTile();
                    tileFreezeHandler.FreezeVisual();
                    tile.tileAnimationHandler.FreezeAnimation();
                    await Task.Delay(100);
                }
            }

            await Task.Yield();
        }
    }
}