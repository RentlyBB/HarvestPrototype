using UnityEngine;

namespace _Scripts.LevelEditor {
    
    #if UNITY_EDITOR

    public class EditorBtn : MonoBehaviour {

        public void SetTileValueToManager(string value) {
            GridManagerEditor.Instance.valueToSave = value;
        }

        public void AddTileValueToManager(string value) {
            GridManagerEditor.Instance.valueToSave += value;
        }
    }
    #endif
}