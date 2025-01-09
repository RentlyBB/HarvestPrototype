using System;
using System.Collections.Generic;
using _Scripts.GameplayCore;
using _Scripts.GridCore;
using _Scripts.PlayerCore;
using _Scripts.TileCore.BaseClasses;
using _Scripts.UnitySingleton;
using UnityEngine;

namespace _Scripts.Managers {
    public class GridManager : MonoSingleton<GridManager> {
        
        public Camera cam;
        [HideInInspector] public LevelData loadedLevelData;  // It must be removed from here. I put it here because EditorHandler counts on GridManager having the level data.
        private Grid<TileGridObject> _grid;
        private MovementHandler _playerMovementHandler;
        private TileTypeParser _tileTypeParser;

        private void OnEnable() {
            GameplayManager.OnLoadLevel += IntiAndFillGrid;
        }

        private void OnDisable() {
            GameplayManager.OnLoadLevel -= IntiAndFillGrid;
        }

        protected override void Awake() {
            base.Awake();
            
            // TODO: this should be anywhere else but here.
            GameObject.FindWithTag("Player").TryGetComponent(out _playerMovementHandler);
            TryGetComponent(out _tileTypeParser);
        }
       
        public void IntiAndFillGrid(LevelData currentLevelData) {
            //I need to do that because of level editor
            loadedLevelData = currentLevelData;
            if (InitGrid()) {
                FillGrid();
            }

            //Set player starting position
            _playerMovementHandler?.SetStartingTile(_grid.GetGridObject(loadedLevelData.startingGridPosition));
            
            //Center the camera
            cam.transform.position = new Vector3((float)loadedLevelData.gridWidth / 2, (float)loadedLevelData.gridHeight / 2, -10);
        }

        //Creates an empty grid with right sizes
        private bool InitGrid() {
            ClearGrid();
            _grid = new Grid<TileGridObject>(loadedLevelData.gridWidth, loadedLevelData.gridHeight, 1, transform.position, (g, x, y) => new TileGridObject(g, x, y));
            return _grid != null;
        }
        
        //Clear the whole grid from all tiles
        private void ClearGrid() {

            if (_grid == null)
                return;

            foreach (KeyValuePair<Vector2Int, TileGridObject> entry in _grid.GetGridDictionary()) {
                Destroy(entry.Value.GetTile().gameObject);
            }
        }

        //Load grid with the tiles from LevelData
        private void FillGrid() {

            // TileData - Holds data for only one tile in the grid 
            foreach (TileData tileData in loadedLevelData.tiles) {
                if (tileData.tileTypeData is null) continue;

                _tileTypeParser.TileTypeToGameObject(tileData, _grid, out TileBase tileBase);
                if (tileBase is  null) break;
                
                tileBase.gridPosition = tileData.gridPosition;
                _grid.GetGridDictionary()[tileData.gridPosition].SetTileBase(tileBase);
                
                tileBase.SetupTile();
                tileBase.tileAnimationHandler.SpawnTileAnimation();
            }
        }

        public Grid<TileGridObject> GetGrid() {
            return _grid;
        }
    }
}