using _Scripts.CustomTools;
using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.TileCore {
    public class TileSoundHandler : MonoBehaviour {
        
        [RequireVariable]
        public AudioClip baseTileSound;

        public void PlaySound(AudioClip clip) {
            AudioManager.Instance.PlaySound(clip);
        }
    }
}