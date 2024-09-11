using UnityEngine;

namespace _Scripts.LevelEditor {
    public class EditorBtn : MonoBehaviour {

        public void SetTileVauleToManager(string value) {
            GridManagerEditor.Instance.valueToSave = value;
        }
    }
}