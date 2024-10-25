using System.Collections.Generic;
using UnityEngine;
using UnitySingleton;

namespace _Scripts {
    public class MusicManager : PersistentMonoSingleton<MusicManager> {

        private AudioSource audioSource;
        public List<AudioClip> playlist = new List<AudioClip>(); // List of songs
        private int currentSongIndex = 0; // Keep track of the current song index


        protected override void Awake() {
            base.Awake();
            audioSource = GetComponent<AudioSource>();
            if (playlist.Count > 0) {
                PlayNextSong(); // Start with the first song
            }
            
            SetMusicVolume(0.05f);
        }

        private void Update() {
            // Check if the song has finished playing
            if (!audioSource.isPlaying && playlist.Count > 0) {
                PlayNextSong();
            }
        }

        // Play the next song in the playlist
        public void PlayNextSong() {
            if (playlist.Count == 0) return;

            // Increment the song index and loop if necessary
            currentSongIndex = (currentSongIndex + 1) % playlist.Count;

            // Play the next song
            audioSource.clip = playlist[currentSongIndex];
            audioSource.Play();
        }

        // Set a custom playlist and start playing from the first song
        public void SetPlaylist(List<AudioClip> newPlaylist) {
            playlist = newPlaylist;
            currentSongIndex = -1; // Reset the index to start from the first song
            PlayNextSong(); // Start playing the new playlist
        }

        // Optionally adjust volume (0 to 1)
        public void SetMusicVolume(float volume) {
            audioSource.volume = Mathf.Clamp01(volume);
        }

        // Optionally stop music
        public void StopMusic() {
            audioSource.Stop();
        }
    }
}