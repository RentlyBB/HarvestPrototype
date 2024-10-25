using _Scripts.GameplayCore.Tiles;
using UnityEngine;

namespace _Scripts.GridCore {
    [System.Serializable]
    public class GridObject{

        private Grid<GridObject> g;
        private int x;
        private int y;
        
        private Tile _tile;

        public GridObject() { 
        
        }

        public GridObject(Grid<GridObject> g, int x, int y) {
            this.g = g;
            this.x = x;
            this.y = y;
        }

        public void SetTile(Tile tile) {
            if(CanCreateTile()) _tile = tile;
        }

        public Tile GetTile() {
            return _tile;
        }

        public void ClearTile() {
            _tile = null;
        }

        public bool CanCreateTile() {
            return _tile == null;
        }

        public int GetX() {
            return x;
        }

        public int GetZ() {
            return y;
        }

        public Grid<GridObject> GetGrid() {
            return g;
        }
        
        public Vector3 GetWorldPositionCellCenter() {
            return g.GetWorldPositionCellCenter(x, y);
        }

        public override string ToString() {
            return x + ", " + y;
        }
    }
}