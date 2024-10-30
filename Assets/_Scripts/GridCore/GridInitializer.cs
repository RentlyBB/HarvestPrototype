using System;
using System.Collections.Generic;
using _Scripts.CustomTools;
using _Scripts.TileCore.BaseClasses;
using UnityEngine;

namespace _Scripts.GridCore {
    public class GridInitializer : MonoBehaviour {

        public int width, height, cellSize;
        public Camera cam;
        private Grid<TileGridObject> _grid;
        
        [RequireVariable]
        public TileBase tilePrefab;
        
        private void Start() {
            InitGrid();
        }

        private void InitGrid() {
            //Creating grid of TileGridObject -> each TileGridObject is now empty and missing the TileCore
            //TODO: Need to create some Level/Grid loader which will fill the TileGridObjects with TileCores
            _grid = new Grid<TileGridObject>(width, height, cellSize, transform.position, (g, x, y) => new TileGridObject(g, x, y));

            var go = _grid.GetGridArray();

            foreach (var gridOb in go) {
                var tile = Instantiate(tilePrefab, gridOb.GetWorldPositionCellCenter(), Quaternion.identity);
                gridOb.SetTileBase(tile);
            }

            cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
        }
    }
}