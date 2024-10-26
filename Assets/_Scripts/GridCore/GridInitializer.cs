using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.GridCore {
    public class GridInitializer : MonoBehaviour {

        public int width, height, cellSize;
        public Camera cam;
        public Grid<TileGridObject> Grid;

        private void Start() {
            InitGrid();
        }

        private void InitGrid() {
            //Creating grid of TileGridObject -> each TileGridObject is now empty and missing the TileCore
            //TODO: Need to create some Level/Grid loader which will fill the TileGridObjects with TileCores
            Grid = new Grid<TileGridObject>(width, height, cellSize, transform.position, (g, x, y) => new TileGridObject(g, x, y));

            Grid.GetGridObject(1, 1);
            
            cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
        }
    }
}