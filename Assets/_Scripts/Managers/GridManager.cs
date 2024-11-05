using System;
using _Scripts.GameplayCore;
using _Scripts.GridCore;
using _Scripts.PlayerCore;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using UnityEngine;
using UnityEngine.Events;
using UnitySingleton;

namespace _Scripts.Managers {
    public class GridManager : PersistentMonoSingleton<GridManager> {

        public static UnityAction<Grid<TileGridObject>> OnGridInit = delegate { };

        public LevelData levelData;

        private Grid<TileGridObject> _grid;
        private MovementHandler _playerMovementHandler;
        private TileTypeParser _tileTypeParser;

        protected override void Awake() {
            base.Awake();
            GameObject.FindWithTag("Player").TryGetComponent(out _playerMovementHandler);
            TryGetComponent(out _tileTypeParser);
        }

        private void Start() {
            LoadLevel();
        }

        public void LoadLevel() {

            if (InitGrid()) {
                FillGrid();
            }

            _playerMovementHandler?.SetStartingTile(_grid.GetGridObject(levelData.startingGridPosition));
        }

        //Creates an empty grid
        public bool InitGrid() {

            if (_grid != null) {
                // TODO: Clear the grid
            }

            _grid = new Grid<TileGridObject>(levelData.gridWidth, levelData.gridHeight, 1, transform.position, (g, x, y) => new TileGridObject(g, x, y));
            if (_grid == null) return false;

            OnGridInit?.Invoke(_grid);
            return true;
        }

        //Load grid with the data from LevelData
        public void FillGrid() {

            // TileData - Holds data for only one tile in the grid 
            foreach (TileData tile in levelData.tiles) {
                _tileTypeParser.TileTypeToGameObject(tile, out TileBase tileBase, _grid);
                tileBase.gridPosition = tile.gridPosition;
                _grid.GetGridDictionary()[tile.gridPosition].SetTileBase(tileBase);
            }
        }

        public void ReplaceTileWith(Vector2Int tileGridPosition, TileType tileToCreate) {
            TileGridObject gridObject = _grid.GetGridDictionary()[tileGridPosition];

            Destroy(gridObject.GetTile().gameObject);

            gridObject.ClearTile();
            _tileTypeParser.TileTypeToGameObject(tileToCreate, tileGridPosition, _grid, out TileBase tileBase);
            gridObject.SetTileBase(tileBase);

        }
    }
}