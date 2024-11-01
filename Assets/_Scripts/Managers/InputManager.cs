using System;
using _Scripts.GridCore;
using _Scripts.InputCore;
using UnityEngine;
using UnityEngine.Events;
using UnitySingleton;

namespace _Scripts.Managers {
    public class InputManager : PersistentMonoSingleton<InputManager> {
        public InputReader inputReader;
        public GridInitializer gridInitializer;

        public static event UnityAction<TileGridObject> ClickOnTile = delegate { };
        
        private void OnEnable() {
            if (inputReader != null) inputReader.MouseClick += OnMouseClick;
        }

        private void OnDisable() {
            if (inputReader != null) inputReader.MouseClick -= OnMouseClick;
        }

        // Inform GameManager that player clicked
        private void OnMouseClick(Vector3 mousePosition) {
            var tileGridObject = gridInitializer.Grid.GetGridObject(mousePosition);

            // Check if player click on GridObject
            if (tileGridObject == null) return; 
            
            //Click on grid
            ClickOnTile?.Invoke(tileGridObject);
        }
    }
}