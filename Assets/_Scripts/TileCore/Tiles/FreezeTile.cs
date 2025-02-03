using System;
using System.Threading.Tasks;
using _Scripts.GridCore;
using _Scripts.Managers;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using UnityEngine;

namespace _Scripts.TileCore.Tiles {

    public enum FreezeDirection {
        Horizontal,
        Vertical
    }
    
    public class FreezeTile : TileBase {
        
        [Space]
        public FreezeDirection freezeDirection;
        private int _axisSize = 0; // size of one axis of the grid, depends on the direction
        private int _gridPositionAxis;
        private Grid<TileGridObject> _grid;
        
        public override void SetupTile() {
            tileVisualHandler.ProcessVisualChange(TileMainVisualStates.DefaultState, TileSubVisualStates.Unpressed);
            
            _grid = GridManager.Instance.GetGrid();
            
            switch (freezeDirection) {
                case FreezeDirection.Horizontal:
                    _gridPositionAxis = gridPosition.y;
                    _axisSize = _grid.GetWidth();
                    break;
                case FreezeDirection.Vertical:
                    _gridPositionAxis = gridPosition.x;
                    _axisSize = _grid.GetHeight();
                    break;
                default:
                    freezeDirection = FreezeDirection.Horizontal;
                    break;
            }
        }
        
        public override async Task OnPlayerStep() {
            await base.OnPlayerStep();
            await FreezeLine();
            await Task.Delay(200);
        }
        
        private Vector2 GetGridCoordinate(int index) {
            return freezeDirection == FreezeDirection.Horizontal
                ? new Vector2(index, _gridPositionAxis)
                : new Vector2(_gridPositionAxis, index);
        }
        
        private async Task FreezeLine() {

            // Freeze Tile
            for (var i = 0; i < _axisSize; i++) {

                var tile = _grid.GetGridObject(GetGridCoordinate(i)).GetTile();
                if (tile.TryGetComponent(out TileFreezeHandler tileFreezeHandler)) {
                    tileFreezeHandler.FreezeTile();
                    tileFreezeHandler.FreezeVisual();
                    tile.tileAnimationHandler?.FreezeAnimation();
                    await Task.Delay(150);
                }
            }
            
            for (var i = 0; i < _axisSize; i++) {
                
                var tile = _grid.GetGridObject(GetGridCoordinate(i)).GetTile();
                if (tile.TryGetComponent(out TileFreezeHandler tileFreezeHandler)) {
                    tile.TryGetComponent(out CountdownTileBase countdownTileBase);
                    if(countdownTileBase && countdownTileBase.countdownState != CountdownState.Collected) return;
                    tileFreezeHandler.UnfreezeTile();
                    tile.tileAnimationHandler?.FreezeAnimation();

                    // skip delay at last tile in grid row/column
                    if (i != _axisSize) {
                        await Task.Delay(150);
                    }
                }
            }

            await Task.Yield();
        }
        
    }
}