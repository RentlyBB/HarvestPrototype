using System;
using _Scripts.TileCore.Enums;
using UnityEngine;
using TMPro;

namespace _Scripts.LevelEditor {
    
    public class BtnUIBehaviour : MonoBehaviour {

        public TileType tileType;

        public TMP_InputField textInput;

        public void SetTileType() {
            LevelEditorManager.Instance.selectedTileType = tileType;
        }

        public void SetCountdownValue(int i) {
            LevelEditorManager.Instance.selectedCountdownValue += i;

            if (textInput is null) {
                Debug.Log("Text Input Box is not set.");
                return;
            }

            textInput.text = LevelEditorManager.Instance.selectedCountdownValue.ToString();
        }

        public void DirectUpdateCountdownValue() {

            if (int.TryParse(textInput.text, out int result)) {
                LevelEditorManager.Instance.selectedCountdownValue = result;
            }
        }

        public void SetStartingPosition() {
            LevelEditorManager.Instance.setStartingPosition = true;
        }

        public void SaveLevel() {
            LevelEditorManager.Instance.SaveLevel();
        }
    }
}