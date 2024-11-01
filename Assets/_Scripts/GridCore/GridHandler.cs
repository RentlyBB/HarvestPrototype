using _Scripts.CustomTools;
using _Scripts.GameplayCore;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.GridCore {
    public class GridHandler : MonoBehaviour {

        [RequireVariable]
        private Grid<TileGridObject> _grid;
        
        public static UnityAction<Grid<TileGridObject>> OnGridInit = delegate { };

        public bool InitGrid(LevelData levelData) {
            _grid = new Grid<TileGridObject>(levelData.gridWidth, levelData.gridHeight, 1, transform.position, (g, x, y) => new TileGridObject(g, x, y));
            
            if (_grid == null) return false;
            
            OnGridInit?.Invoke(_grid);

            return true;
        }

        public Grid<TileGridObject> GetGrid() {
            return _grid;
        }
    }
}