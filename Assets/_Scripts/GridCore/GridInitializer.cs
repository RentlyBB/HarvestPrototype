using System;
using System.Collections.Generic;
using _Scripts.GameplayCore.Tiles;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.GridCore {
    public class GridInitializer : MonoBehaviour {

        public int width, height, cellSize;
        public Camera cam;
        public Grid<TileGridObject> Grid;
        public LevelDataSo levelDataSo;

        private void Start() {
            InitGrid();
        }

        private void InitGrid() {
            //Creating grid of TileGridObject -> each TileGridObject is now empty and missing the TileCore
            //TODO: Need to create some Level/Grid loader which will fill the TileGridObjects with TileCores
            Grid = new Grid<TileGridObject>(width, height, cellSize, transform.position, (g, x, y) => new TileGridObject(g, x, y));

            LevelDataObj levelDataObj = new LevelDataObj();


            // levelDataObj.tiles.Add( new BlankTile(0,0));
            // levelDataObj.tiles.Add(new BlankTile(1, 0));
            // levelDataObj.tiles.Add(new BlankTile(2, 0));
            // levelDataObj.tiles.Add(new BlankTile(3, 0));
            
            
            cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
        }
    }
}