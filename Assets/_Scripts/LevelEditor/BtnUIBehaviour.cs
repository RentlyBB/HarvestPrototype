using _Scripts.TileCore.Enums;
using UnityEngine;

namespace _Scripts.LevelEditor {
    public class BtnUIBehaviour : MonoBehaviour {
        
        public TileType tileType;

        public void SetTileType() {
            LevelEditorManager.Instance.selectedTileType = tileType;
        }

    }
}