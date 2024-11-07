using System;
using _Scripts.GridCore;
using _Scripts.InputCore;
using UnityEngine;
using UnityEngine.Events;
using UnitySingleton;

namespace _Scripts.Managers {
    public class InputManager : PersistentMonoSingleton<InputManager> {
        public InputReader inputReader;
        public static event UnityAction<TileGridObject> OnClickOnTile = delegate { };

        private Grid<TileGridObject> _grid;
        
        private void OnEnable() {
            if (inputReader != null) inputReader.OnMouseClick += OnMouseClick;
            GridManager.GridInit += SetGrid;
        }

        private void OnDisable() {
            if (inputReader != null) inputReader.OnMouseClick -= OnMouseClick;
            GridManager.GridInit -= SetGrid;
        }

        // Inform GameManager that player clicked
        private void OnMouseClick(Vector3 mousePosition) {
            var tileGridObject = _grid?.GetGridObject(mousePosition);
            
            // Check if player click on GridObject
            if (tileGridObject == null) return; 
            
            //Click on grid
            OnClickOnTile?.Invoke(tileGridObject);
        }

        private void SetGrid(Grid<TileGridObject> grid) {
            _grid = grid;
        }
    }
}