using _Scripts.TileCore;
using _Scripts.TileCore.BaseClasses;
using UnityEngine;

namespace _Scripts.GridCore {
    public class TileGridObject {
        private Grid<TileGridObject> g;
        private int x;
        private int y;

        private TileBase _tileBase;

        public TileGridObject() { }

        public TileGridObject(Grid<TileGridObject> g, int x, int y) {
            this.g = g;
            this.x = x;
            this.y = y;
        }

        public void SetTileBase(TileBase tileBase) {
            if (CanCreateTile()) _tileBase = tileBase;
        }

        public TileBase GetTile() {
            return _tileBase;
        }

        public void ClearTile() {
            _tileBase = null;
        }

        public bool CanCreateTile() {
            return _tileBase == null;
        }

        public int GetX() {
            return x;
        }

        public int GetY() {
            return y;
        }

        public Vector2Int GetXY() {
            return new Vector2Int(x, y);
        }

        public Grid<TileGridObject> GetGrid() {
            return g;
        }

        public Vector3 GetWorldPositionCellCenter() {
            return g.GetWorldPositionCellCenter(x, y);
        }
        
        public Vector3 GetWorldPosition() {
            return g.GetWorldPosition(x, y);
        }

        public override string ToString() {
            return x + ", " + y;
        }
    }
}