using _Scripts.Managers;
using UnityEngine;

namespace _Scripts.TileCore {
    public class TileSoundHandler : MonoBehaviour {
        
        public AudioClip baseTileSound;

        public void PlaySound(AudioClip clip) {
            AudioManager.Instance.PlaySound(clip);
        }
    }
}