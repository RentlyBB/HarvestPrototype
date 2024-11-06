using System;
using _Scripts.GameplayCore;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using _Scripts.TileCore.Tiles;
using UnityEngine;

namespace _Scripts.GridCore {
    public class TileTypeParser : MonoBehaviour {
        private const string DefaultTile = "TilePrefabs/DefaultTile";
        private const string CountdownTile = "TilePrefabs/CountdownTile";
        private const string CollectTile = "TilePrefabs/CollectTile";
        private const string BadCollectTile = "TilePrefabs/BadCollectTile";


        // Get TileType variable and creates a GameObject of that type
        public void TileTypeToGameObject(TileData tileData, out TileBase tileBase, Grid<TileGridObject> grid) {
            tileBase = null;

            switch (tileData.tileType) {
                case TileType.EmptyTile:
                    break;
                case TileType.DefaultTile:
                    tileBase = Instantiate(Resources.Load<GameObject>(DefaultTile), grid.GetWorldPositionCellCenter(tileData.gridPosition), Quaternion.identity, transform).GetComponent<TileBase>();
                    break;
                case TileType.CountdownTile:
                    tileBase = Instantiate(Resources.Load<GameObject>(CountdownTile), grid.GetWorldPositionCellCenter(tileData.gridPosition), Quaternion.identity, transform).GetComponent<TileBase>();
                    tileBase.GetComponent<CountdownTile>().countdownValue = tileData.countdownValue;
                    break;
                case TileType.FreezeTile:
                    break;
                case TileType.PushTile:
                    break;
                case TileType.CollectTile:
                    tileBase = Instantiate(Resources.Load<GameObject>(CollectTile), grid.GetWorldPositionCellCenter(tileData.gridPosition), Quaternion.identity, transform).GetComponent<TileBase>();
                    break;
                case TileType.BadCollectTile:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tileData.tileType), tileData.tileType, null);
            }
        }

        public void TileTypeToGameObject(TileType tileType, Vector2Int pos, Grid<TileGridObject> grid, out TileBase tileBase) {
            tileBase = null;
            switch (tileType) {
                case TileType.DefaultTile:
                    tileBase = Instantiate(Resources.Load<GameObject>(DefaultTile), grid.GetWorldPositionCellCenter(pos), Quaternion.identity, transform).GetComponent<TileBase>();
                    tileBase.gridPosition = pos;
                    break;
                case TileType.CollectTile:
                    tileBase = Instantiate(Resources.Load<GameObject>(CollectTile), grid.GetWorldPositionCellCenter(pos), Quaternion.identity, transform).GetComponent<TileBase>();
                    tileBase.gridPosition = pos;
                    break;
                case TileType.BadCollectTile:
                    tileBase = Instantiate(Resources.Load<GameObject>(BadCollectTile), grid.GetWorldPositionCellCenter(pos), Quaternion.identity, transform).GetComponent<TileBase>();
                    tileBase.gridPosition = pos;
                    break;
                case TileType.EmptyTile:
                case TileType.CountdownTile:
                case TileType.FreezeTile:
                case TileType.PushTile:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tileType), tileType, null);
            }
        }
    }
}