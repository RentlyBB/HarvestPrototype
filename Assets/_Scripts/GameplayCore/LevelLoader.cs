using System;
using _Scripts.GridCore;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using UnityEngine;

namespace _Scripts.GameplayCore {
    public class LevelLoader : MonoBehaviour {
        public void LoadLevel(Grid<TileGridObject> grid, LevelData levelData) {
            foreach (TileData tile in levelData.tiles) {
                GameObject tileBase = TileTypeToGameObject(grid, tile);
                grid.GetGridDictionary()[tile.gridPosition].SetTileBase(tileBase);
            }
        }

        private GameObject TileTypeToGameObject(Grid<TileGridObject> grid, TileData tileData) {
            GameObject tile = null;

            switch (tileData.tileType) {
                case TileType.EmptyTile:
                    return null;
                case TileType.DefaultTile:
                    return Instantiate(Resources.Load<GameObject>("TilePrefabs/DefaultTile"), grid.GetWorldPositionCellCenter(tileData.gridPosition), Quaternion.identity, transform);
                case TileType.CountdownTile:
                    tile = Instantiate(Resources.Load<GameObject>("TilePrefabs/CountdownTile"), grid.GetWorldPositionCellCenter(tileData.gridPosition), Quaternion.identity, transform);
                    tile.GetComponent<CountdownTileBase>().countdownValue = tileData.countdownValue;
                    return tile;
                case TileType.DoubleCountdownTile:
                    return null;
                case TileType.FreezeTile:
                    return null;
                case TileType.PushTile:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tileData.tileType), tileData.tileType, null);
            }
        }

    }
}