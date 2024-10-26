using _Scripts.GameplayCore.Tiles;
using UnityEngine;

namespace _Scripts.GridCore {
    public class TileGridObject {
        
        private Grid<TileGridObject> g;
        private int x;
        private int y;
        
        private TileCore _tileCore;

        public TileGridObject() { 
        
        }

        public TileGridObject(Grid<TileGridObject> g, int x, int y) {
            this.g = g;
            this.x = x;
            this.y = y;
        }

        public void SetTileCore(TileCore tileCore) {
            if(CanCreateTile()) _tileCore = tileCore;
        }

        public TileCore GetTile() {
            return _tileCore;
        }

        public void ClearTile() {
            _tileCore = null;
        }

        public bool CanCreateTile() {
            return _tileCore == null;
        }

        public int GetX() {
            return x;
        }

        public int GetZ() {
            return y;
        }

        public Grid<TileGridObject> GetGrid() {
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