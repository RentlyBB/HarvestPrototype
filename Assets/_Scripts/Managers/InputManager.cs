using System;
using _Scripts.GridCore;
using _Scripts.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnitySingleton;

namespace _Scripts.Managers {
    public class InputManager : MonoSingleton<InputManager>, GameInput.IGameplayActions {
        public static event UnityAction<TileGridObject> OnClickOnTile = delegate { };

        private GameInput _gameInput;

        private Grid<TileGridObject> _grid;

        private void OnEnable() {

            if (_gameInput == null) {
                _gameInput = new GameInput();
                _gameInput.Gameplay.SetCallbacks(this);
            }

            _gameInput.Gameplay.Enable();

            GridManager.GridInit += SetGrid;
        }

        private void OnDisable() {

            _gameInput.Gameplay.Disable();
            GridManager.GridInit -= SetGrid;
        }

        private void SetGrid(Grid<TileGridObject> grid) {
            _grid = grid;
        }

        public void OnMouseClick(InputAction.CallbackContext context) {
            if (context.phase == InputActionPhase.Performed) {
                var tileGridObject = _grid?.GetGridObject(Utils.GetMouseWorldPosition2D());

                // Check if player click on GridObject
                if (tileGridObject is null) return;
                if(tileGridObject.GetTile().canMoveOn is false) return;

                //Click on grid
                OnClickOnTile?.Invoke(tileGridObject);
            }
        }
    }
}