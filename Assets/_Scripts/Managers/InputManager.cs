using System;
using _Scripts.GridCore;
using _Scripts.InputCore;
using UnityEngine;
using UnitySingleton;

namespace _Scripts.Managers {
    public class InputManager : PersistentMonoSingleton<InputManager> {
        public InputReader inputReader;
        public GridInitializer gridInitializer;

        private void OnEnable() {
            if (inputReader != null) inputReader.MouseClick += OnMouseClick;
        }

        private void OnDisable() {
            if (inputReader != null) inputReader.MouseClick -= OnMouseClick;
        }

        // Inform GameManager that player clicked
        private void OnMouseClick(Vector3 mousePosition) {
            var gridObject = gridInitializer.Grid.GetGridObject(mousePosition);
            
            // Check if player click on GridObject
            if (gridObject != null) {
                //Click on grid
                Debug.Log(gridObject.GetX() + ", " + gridObject.GetY());
            } else {
                Debug.Log("Out of the grid");
            }
        }
    }
}