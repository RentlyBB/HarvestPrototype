using UnityEngine;

namespace _Scripts.LevelEditor {
    public class EditorBtn : MonoBehaviour {

        public void SetTileValueToManager(string value) {
            GridManagerEditor.Instance.valueToSave = value;
        }

        public void AddTileValueToManager(string value) {
            GridManagerEditor.Instance.valueToSave += value;
        }
    }
}