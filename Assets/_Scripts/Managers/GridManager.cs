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
        
        public static UnityAction<Grid<TileGridObject>> GridInit = delegate { };

        public LevelData levelData;

        public Camera cam;
        
        private Grid<TileGridObject> _grid;
        private MovementHandler _playerMovementHandler;
        private TileTypeParser _tileTypeParser;

        protected override void Awake() {
            base.Awake();
            GameObject.FindWithTag("Player").TryGetComponent(out _playerMovementHandler);
            TryGetComponent(out _tileTypeParser);
        }
       
        private void Start() {
            foreach (TileData tileData in levelData.tiles) {
                if (tileData.tileType is (TileType.CountdownTile or TileType.RepeatCountdownTile)) {
                    Debug.Log("+1");
                }
            }
            
            LoadLevel();
            cam.transform.position = new Vector3((float)levelData.gridWidth / 2, (float)levelData.gridHeight / 2, -10);
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

            GridInit?.Invoke(_grid);
            return true;
        }

        //Load grid with the data from LevelData
        public void FillGrid() {

            // TileData - Holds data for only one tile in the grid 
            foreach (TileData tile in levelData.tiles) {
                _tileTypeParser.TileTypeToGameObject(tile, out TileBase tileBase, _grid);
                if (tileBase is  null) break;
                
                tileBase.gridPosition = tile.gridPosition;
                _grid.GetGridDictionary()[tile.gridPosition].SetTileBase(tileBase);
            }
        }

        public void ReplaceAndDestroyTileWith(Vector2Int tileGridPosition, TileType tileToCreate) {
            TileGridObject gridObject = _grid.GetGridDictionary()[tileGridPosition];

            Destroy(gridObject.GetTile().gameObject);

            gridObject.ClearTile();
            _tileTypeParser.TileTypeToGameObject(tileToCreate, tileGridPosition, _grid, out TileBase tileBase);
            gridObject.SetTileBase(tileBase);

        }
    }
}