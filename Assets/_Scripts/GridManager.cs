using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts.Singleton;
using _Scripts.SOs;
using QFSW.QC;

namespace _Scripts {

    public class GridManager : Singleton<GridManager> {

        [SerializeField] private Tile _tilePrefab;

        [SerializeField] private Transform _cam;

        private Dictionary<Vector2, Tile> _tiles;

        private int _width, _height;

        private List<string> _tileData = new List<string>();


        public bool LoadGridData(GridLevelData gridLevelData) {

            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }

            _width = gridLevelData.gridSize.x;
            _height = gridLevelData.gridSize.y;
            _tileData = gridLevelData.tileData;
            return true;
        }

        [Command]
        public void Test(string s) {
            Debug.Log(s);
        }

        public void GenerateGrid() {
            _tiles = new Dictionary<Vector2, Tile>();
            int i = 0;
            for (int x = 0; x < _width; x++) {
                for (int y = 0; y < _height; y++) {
                    var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity);
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
        }

        public Tile GetTileAtPosition(Vector2 pos) {
            if (_tiles.TryGetValue(pos, out var tile)) return tile;

            return null;
        }
    }
}