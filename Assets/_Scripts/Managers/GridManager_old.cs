using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts.SOs;
using QFSW.QC;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnitySingleton;

namespace _Scripts {

    public class GridManager_old : MonoSingleton<GridManager_old> {

        [FormerlySerializedAs("_tilePrefab")]
        [SerializeField] private Tile_old tileOldPrefab;

        [SerializeField] private Transform _cam;

        private Dictionary<Vector2, Tile_old> _tiles;

        public int _width, _height;

        private List<string> _tileData = new List<string>();
        
        private void OnEnable() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public bool LoadGridData(GridLevelData gridLevelData) {

            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }

            _width = gridLevelData.gridSize.x;
            _height = gridLevelData.gridSize.y;
            _tileData = gridLevelData.tileData;
            return true;
        }

        public void GenerateGrid() {
            _tiles = new Dictionary<Vector2, Tile_old>();
            int i = 0;
            for (int x = 0; x < _width; x++) {
                for (int y = 0; y < _height; y++) {
                    var spawnedTile = Instantiate(tileOldPrefab, new Vector3(x, y), Quaternion.identity);
                    spawnedTile.transform.SetParent(transform);
                    spawnedTile.name = $"Tile {x} {y}";

                    var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                    spawnedTile.gridPosition = new Vector2Int(x, y);
                    
                    spawnedTile.Init(isOffset, _tileData[i]);
                    _tiles[new Vector2(x, y)] = spawnedTile;
                    i++;
                }
            }

            _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);

            //transform.position = new Vector3(_cam.transform.position.x - (float)(_width / 2 - 0.5) , _cam.transform.position.y - (float)(_height / 2 - 0.5), transform.position.z);
        }

        public Tile_old GetTileAtPosition(Vector2 pos) {
            if (_tiles.TryGetValue(pos, out var tile)) return tile;

            return null;
        }

        public void GetAllInRow(int rowNumber, out List<Tile_old> tiles) {
            var allTiles = new List<Tile_old>();

            for (int i = 0; i < _width; i++) {
                allTiles.Add(GetTileAtPosition(new Vector2(i, rowNumber)));
            }

            tiles = allTiles;
        }
        
        public void GetAllInColumn(int columnNumber, out List<Tile_old> tiles) {
            var allTiles = new List<Tile_old>();

            for (int i = 0; i < _height; i++) {
                allTiles.Add(GetTileAtPosition(new Vector2(columnNumber, i)));
            }

            tiles = allTiles;
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            LoadGridData(GameManager_old.Instance.GetLevel());
            GenerateGrid();
            GameManager_old.Instance.SetPlayerPosition();
        }
    }
}