using UnityEngine;
using UnitySingleton;

namespace _Scripts.Managers {
    public class AudioManager : PersistentMonoSingleton<AudioManager> {
        private AudioSource _audioSource;

        protected override void Awake() {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
            SetVolume(0.5f);
        }

        // Method to play sound
        public void PlaySound(AudioClip clip) {
            _audioSource.PlayOneShot(clip); // Play the sound once
        }

        // Method to set the volume (0 to 1)
        public void SetVolume(float volume) {
            _audioSource.volume = Mathf.Clamp01(volume);
        }
    }
}