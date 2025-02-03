using System.Threading.Tasks;
using _Scripts.Managers;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using UnityEngine;

namespace _Scripts.TileCore.Tiles {
    public class FreezeHorizontalTile : TileBase {

        public override void SetupTile() {
            tileVisualHandler.ProcessVisualChange(TileMainVisualStates.DefaultState, TileSubVisualStates.Unpressed);
        }

        public override async Task OnPlayerStep() {
            await base.OnPlayerStep();
            await FreezeLine();
            await Task.Delay(200);
        }

        private async Task FreezeLine() {
            var grid = GridManager.Instance.GetGrid();

            // Freeze Tile
            for (var i = 0; i < grid.GetWidth(); i++) {
                var tile = grid.GetGridObject(new Vector2(i, gridPosition.y)).GetTile();
                if (tile.TryGetComponent(out TileFreezeHandler tileFreezeHandler)) {
                    tileFreezeHandler.FreezeTile();
                    tileFreezeHandler.FreezeVisual();
                    tile.tileAnimationHandler?.FreezeAnimation();
                    await Task.Delay(150);
                }
            }
            
            for (var i = 0; i < grid.GetWidth(); i++) {
                var tile = grid.GetGridObject(new Vector2(i, gridPosition.y)).GetTile();
                if (tile.TryGetComponent(out TileFreezeHandler tileFreezeHandler)) {
                    
                    tile.TryGetComponent(out CountdownTileBase countdownTileBase);
                    if(countdownTileBase && countdownTileBase.countdownState != CountdownState.Collected) return;
                    
                    tileFreezeHandler.UnfreezeTile();
                    tile.tileAnimationHandler?.FreezeAnimation();
                    await Task.Delay(150);
                }
            }

            await Task.Yield();
        }
    }
}