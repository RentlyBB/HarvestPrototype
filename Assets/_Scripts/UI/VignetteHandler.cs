using _Scripts.GridCore;
using _Scripts.Managers;
using _Scripts.Utilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts.UI {
    public class VignetteHandler : MonoBehaviour {
        
        private void OnEnable() {
            InputManager.OnClickOnTile1 += ClickOn;
        }

        private void OnDisable() {
            InputManager.OnClickOnTile1 -= ClickOn;
        }

        public void ClickOn() {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            mousePos = new Vector2(mousePos.x / Screen.width, mousePos.y / Screen.height);
            Debug.Log("Mouse Screen Position: " + mousePos);
        }
    }
}