using System;
using System.Collections.Generic;
using _Scripts.GameplayCore.Tiles;
using QFSW.QC;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.GridCore {
    public class GridInitializer : MonoBehaviour {
        
        //TODO: Udělat TileGridObject –> která je obalka pro grid.

        public int width, height, cellSize;
        public Camera cam;
        public Grid<Tile> Grid = default;


        public List<Tile> gridObjects = new List<Tile>();

        private void Start() {
            InitGrid();
        }

        private void InitGrid() {
            //Grid = new Grid<Tile>(width, height, cellSize, transform.position, (g, x, y) => new Tile(g, x, y));
            
            
            //TODO: DataSet, ktery drzi informace kde jakej tile je.
            // DataSet musi obsahovat array Tile, kazdy tile uz obsahuje pozici na gridu.
            
            cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
        }
        
        [Command]
        public void ListsGridObjects() {
            foreach (var gridObject in Grid.GetGridArray()) {
                Debug.Log(gridObject.ToString());
            }
        }
    }
}