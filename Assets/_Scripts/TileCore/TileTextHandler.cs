using TMPro;
using UnityEngine;

namespace _Scripts.TileCore {
    public class TileTextHandler : MonoBehaviour {
        
        public TextMeshPro middleText;

        public void AddText(string text) {
            middleText = Utils.CreateTextWorld(text, new Vector3(transform.position.x, transform.position.y + 0.05f, -1), 40, transform, Color.green);
        }

        public void RemoveText() {
            Destroy(middleText);
        }

        public void UpdateText(string text) {
            if (middleText is not null) {
                middleText.text = text;
            }
        }
    }
}