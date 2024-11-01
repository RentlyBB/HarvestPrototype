using System;
using System.Collections.Generic;
using _Scripts.GridCore;
using UnityEngine;

namespace _Scripts.GameplayCore {
    [RequireComponent(typeof(LevelParser))]
    public class LevelLoader : MonoBehaviour {

        private LevelParser _levelParser;

        private void Awake() {
            TryGetComponent(out _levelParser);
        }

        public void LoadLevel(Grid<TileGridObject> grid, LevelData levelData) {
            foreach (TileData tile in levelData.tiles) {
                GameObject tileBase = Instantiate(_levelParser.TileTypeToGameObject(tile.tileType), grid.GetWorldPositionCellCenter(tile.gridPosition), Quaternion.identity, transform);
                grid.GetGridDictionary()[tile.gridPosition].SetTileBase(tileBase);
            }
        }
    }
}