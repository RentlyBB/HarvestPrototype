using System.Collections.Generic;
using _Scripts.Managers;
using _Scripts.TileCore.BaseClasses;
using UnityEngine;

namespace _Scripts.TileCore.Tiles {
    public class FreezeTile : TileBase {
        
        
        
        private void FreezeLine() {
            // if (_tileType == TileType.FreezeTileHorizontal) {
            //     // Using GetRow method to get all tiles in row
            //     GridManager.Instance.GetAllInRow(gridPosition.y, out var allTiles);
            //     foreach (var tile in allTiles) {
            //         tile._isFreeze = true;
            //         if (tile._tileState is TileState.Normal or TileState.Exclamation or TileState.Pushing) {
            //             tile._tileState = TileState.Freeze;
            //             tile.ChangeTileSprite();
            //         }
            //     }
            // } else if (_tileType == TileType.FreezeTileVertical) {
            //     // Using GetColumn method to get all tiles in column
            //     GridManager.Instance.GetAllInColumn(gridPosition.x, out var allTiles);
            //     foreach (var tile in allTiles) {
            //         tile._isFreeze = true;
            //         if (tile._tileState is TileState.Normal or TileState.Exclamation or TileState.Pushing) {
            //             tile._tileState = TileState.Freeze;
            //             tile.ChangeTileSprite();
            //         }
            //     }
            // }
        }
        
        public void FreezeRow(out List<TileBase> tiles) {

            var grid = GridManager.Instance.GetGrid();
            
            var allTiles = new List<TileBase>();

            for (int i = 0; i < grid.GetWidth(); i++) {
                allTiles.Add(grid.GetGridObject(new Vector2(i, gridPosition.y)).GetTile());
            }

            tiles = allTiles;
        }
        
        public void FreezeColumn(out List<TileBase> tiles) {
            
            var grid = GridManager.Instance.GetGrid();
            var allTiles = new List<TileBase>();

            for (int i = 0; i < grid.GetHeight(); i++) {
                allTiles.Add(grid.GetGridObject(new Vector2(gridPosition.x, i)).GetTile());
            }

            tiles = allTiles;
        }
    }
}