using System;
using System.Collections.Generic;
using _Scripts.CustomTools;
using EditorScripts;
using UnityEngine;

namespace _Scripts.GridCore {
    public class GridInitializer : MonoBehaviour {

        public int width, height, cellSize;
        public Camera cam;
        public Grid<TileGridObject> Grid;
        
        [RequireVariable]
        public GameObject tilePrefab;
        
        private void Start() {
            InitGrid();
        }
        
        [InvokeButton]
        private void InitGrid() {
            Grid = new Grid<TileGridObject>(width, height, cellSize, transform.position, (g, x, y) => new TileGridObject(g, x, y));

            
            // Access the grid dictionary
            var gridDictionary = Grid.GetGridDictionary();

            foreach (var entry in gridDictionary) {
                // entry.Key is the Vector2Int position, and entry.Value is the TGridObject at that position
                Vector2Int gridPosition = entry.Key;
                var gridObject = entry.Value;
                
                // Get the world position for the cell center
                Vector3 worldPosition = Grid.GetWorldPositionCellCenter(gridPosition.x, gridPosition.y);

                // Instantiate the tile at the cell center and log to confirm instantiation
                var tile = Instantiate(tilePrefab, worldPosition, Quaternion.identity);
                gridObject.SetTileBase(tile); // Assuming SetTileBase sets the tile instance in the grid object
            }

            cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
        }
    }
}