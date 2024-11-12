using _Scripts.GridCore;
using _Scripts.TileCore.BaseClasses;
using UnityEngine;

namespace _Scripts.LevelEditor {
    public class EditorTileGridObject {
        private Grid<EditorTileGridObject> g;
        private int x;
        private int y;

        private EditorTile _editorTile;

        public EditorTileGridObject() { }

        public EditorTileGridObject(Grid<EditorTileGridObject> g, int x, int y) {
            this.g = g;
            this.x = x;
            this.y = y;
        }

        public void SetEditorTile(EditorTile editorTile) {
            if (CanCreateTile()) _editorTile = editorTile;
        }

        public EditorTile GetTile() {
            return _editorTile;
        }

        public void ClearTile() {
            _editorTile = null;
        }

        public bool CanCreateTile() {
            return _editorTile is null;
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

        public Grid<EditorTileGridObject> GetGrid() {
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