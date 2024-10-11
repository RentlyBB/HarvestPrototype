using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace _Scripts.InputCore {
    [CreateAssetMenu(fileName = "InputReader", menuName = "Custom/Input Reader")]
    public class InputReader : ScriptableObject, GameInput.IGameplayActions {
        public GameInput GameInput;

        private void OnEnable() {
            if (GameInput == null) {
                GameInput = new GameInput();
                GameInput.Gameplay.SetCallbacks(this);
            }

            GameInput.Gameplay.Enable();
        }

        private void OnDisable() {
            GameInput.Gameplay.Disable();
        }

        /*GAMEPLAY*/
        public void OnMovement(InputAction.CallbackContext context) {
            if (context.phase == InputActionPhase.Performed) {
                OnMovementE?.Invoke(Utils.GetMousePosition2D());
            }
        }

        // Events for each player input
        public event UnityAction<Vector3> OnMovementE = delegate { };
      

        public void EnableGameplayInput() {
            GameInput.Gameplay.Enable();
        }
    }
}