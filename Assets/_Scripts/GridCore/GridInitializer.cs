using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.GridCore {
    public class GridInitializer : MonoBehaviour {


        public Grid<TileGridObject> InitGrid(int width, int height, int cellSize) {
            
        
            return new Grid<TileGridObject>(width, height, cellSize, transform.position, (g, x, y) => new TileGridObject(g, x, y));
        }
    }
}