using System;
using _Scripts.GridCore;
using _Scripts.InputCore;
using UnityEngine;
using UnityEngine.Events;
using UnitySingleton;

namespace _Scripts.Managers {
    public class InputManager : PersistentMonoSingleton<InputManager> {
        public InputReader inputReader;
        public static event UnityAction<TileGridObject> ClickOnTile = delegate { };

        private Grid<TileGridObject> _grid;
        
        private void OnEnable() {
            if (inputReader != null) inputReader.MouseClick += OnMouseClick;
            GridManager.OnGridInit += SetGrid;
        }

        private void OnDisable() {
            if (inputReader != null) inputReader.MouseClick -= OnMouseClick;
            GridManager.OnGridInit -= SetGrid;
        }

        // Inform GameManager that player clicked
        private void OnMouseClick(Vector3 mousePosition) {
            
            if(_grid == null) return;
            
            var tileGridObject = _grid.GetGridObject(mousePosition);
            
            // Check if player click on GridObject
            if (tileGridObject == null) return; 
            
            //Click on grid
            ClickOnTile?.Invoke(tileGridObject);
        }

        private void SetGrid(Grid<TileGridObject> grid) {
            _grid = grid;
        }
    }
}