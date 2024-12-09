using System;
using System.Collections.Generic;
using _Scripts.GameplayCore;
using _Scripts.GridCore;
using _Scripts.PlayerCore;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Enums;
using QFSW.QC;
using UnityEngine;
using UnityEngine.Events;
using UnitySingleton;

namespace _Scripts.Managers {
    public class GridManager : MonoSingleton<GridManager> {
        
        public static UnityAction<Grid<TileGridObject>> GridInit = delegate { };

        public LevelData currentLevelData;

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
            // foreach (TileData tileData in currentLevelData.tiles) {
            //     if (tileData.tileType is (TileType.CountdownTile or TileType.RepeatCountdownTile)) {
            //         //Debug.Log("+1");
            //     }
            // }
            
            LoadLevel();
            cam.transform.position = new Vector3((float)currentLevelData.gridWidth / 2, (float)currentLevelData.gridHeight / 2, -10);
        }

        
        [Command]
        public void LoadLevel() {

            if (InitGrid()) {
                FillGrid();
            }

            _playerMovementHandler?.SetStartingTile(_grid.GetGridObject(currentLevelData.startingGridPosition));
        }

        //Creates an empty grid
        private bool InitGrid() {

            
            ClearGrid();

            _grid = new Grid<TileGridObject>(currentLevelData.gridWidth, currentLevelData.gridHeight, 1, transform.position, (g, x, y) => new TileGridObject(g, x, y));
            if (_grid == null) return false;

            GridInit?.Invoke(_grid);
            return true;
        }
        private void ClearGrid() {

            if (_grid == null)
                return;

            foreach (KeyValuePair<Vector2Int, TileGridObject> entry in _grid.GetGridDictionary()) {
                Destroy(entry.Value.GetTile().gameObject);
            }
        }

        //Load grid with the data from LevelData
        public void FillGrid() {

            // TileData - Holds data for only one tile in the grid 
            foreach (TileData tileData in currentLevelData.tiles) {
                if (tileData.tileTypeData is null) continue;

                _tileTypeParser.TileTypeToGameObject(tileData, _grid, out TileBase tileBase);
                if (tileBase is  null) break;
                
                tileBase.gridPosition = tileData.gridPosition;
                _grid.GetGridDictionary()[tileData.gridPosition].SetTileBase(tileBase);

                tileBase.SetupTile();
                
                tileBase.tileAnimationHandler.SpawnTile();
            }
        }

        public Grid<TileGridObject> GetGrid() {
            return _grid;
        }
    }
}