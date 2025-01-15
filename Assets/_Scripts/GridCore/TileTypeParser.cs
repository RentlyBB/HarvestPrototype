using System;
using _Scripts.GameplayCore;
using _Scripts.TileCore.BaseClasses;
using UnityEngine;

namespace _Scripts.GridCore {
    public class TileTypeParser : MonoBehaviour {

        // Get TileType variable and creates a GameObject of that type
        // Used for Init load grid
        public void TileTypeToGameObject(TileData tileData, Grid<TileGridObject> grid, out TileBase tileBase) {
            tileBase = null;

            if (tileData.tileTypeData == null || tileData.tileTypeData.tilePrefab == null) {
                Debug.LogError("TileTypeData or prefab is missing!");
                return;
            }

            // Instantiate the tile prefab
            GameObject tileObject = Instantiate(
                tileData.tileTypeData.tilePrefab,
                grid.GetWorldPositionCellCenter(tileData.gridPosition),
                Quaternion.identity,
                transform
            );

            tileBase = tileObject.GetComponent<TileBase>();

            // Apply specific tile properties (e.g., countdown)
            if (tileBase is CountdownTileBase countdownTile) {
                countdownTile.countdownValue = tileData.countdownValue > 0
                    ? tileData.countdownValue
                    : tileData.tileTypeData.defaultCountdownValue;
            }
        }
    }
}