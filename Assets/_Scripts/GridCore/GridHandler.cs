using _Scripts.CustomTools;
using _Scripts.GameplayCore;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts.GridCore {
    [RequireComponent(typeof(GridInitializer))]
    public class GridHandler : MonoBehaviour {

        [RequireVariable]
        private Grid<TileGridObject> _grid;
        private GridInitializer _gridInitializer;
        
        public static UnityAction<Grid<TileGridObject>> OnGridInit = delegate { };

        

        private void Awake() {
            TryGetComponent(out _gridInitializer);
        }

        public bool InitGridLevel(LevelData levelData) {
            _grid = _gridInitializer.InitGrid(levelData.gridWidth, levelData.gridHeight, 1);

            if (_grid == null) return false;
            
            OnGridInit?.Invoke(_grid);

            return true;
        }

        public Grid<TileGridObject> GetGrid() {
            return _grid;
        }
    }
}