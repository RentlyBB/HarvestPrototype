using System;
using System.Collections;
using _Scripts.UnitySingleton;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnitySingleton;

namespace _Scripts.Managers {
    public class CustomSceneManager : PersistentMonoSingleton<CustomSceneManager> {
        public event Action<string> OnSceneLoaded;
        public event Action<string> OnSceneUnloaded;

        private string _currentScene;
        private const string TransitionSceneName = "TransitionScene"; // Set this to the name of your transition scene

        protected override void Awake() {
            base.Awake();
            _currentScene = SceneManager.GetActiveScene().name; // Initialize with the active scene
        }

        /// <summary>
        /// Loads a new scene by name, and automatically unloads the previous scene.
        /// </summary>
        public void LoadScene(string sceneName, bool showLoadingScreen = false) {
            if (sceneName == _currentScene) {
                Debug.LogWarning($"Scene '{sceneName}' is already loaded.");
                return;
            }

            StartCoroutine(SwitchSceneAsync(sceneName, showLoadingScreen));
        }

        /// <summary>
        /// Coroutine to switch scenes, using a temporary transition scene to ensure no errors.
        /// </summary>
        private IEnumerator SwitchSceneAsync(string newSceneName, bool showLoadingScreen) {
            // Show the loading screen if enabled
            if (showLoadingScreen) {
                ShowLoadingScreen();
            }

            // Load the transition scene first to ensure there's always one scene loaded
            AsyncOperation transitionLoad = SceneManager.LoadSceneAsync(TransitionSceneName, LoadSceneMode.Additive);
            yield return new WaitUntil(() => transitionLoad.isDone);

            // Unload the current scene if it exists
            if (!string.IsNullOrEmpty(_currentScene)) {
                yield return StartCoroutine(UnloadSceneAsync(_currentScene));
            }

            // Load the new scene additively
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(newSceneName, LoadSceneMode.Additive);
            loadOperation.allowSceneActivation = false;

            // Wait until the scene is mostly loaded
            while (loadOperation.progress < 0.9f) {
                yield return null;
            }

            loadOperation.allowSceneActivation = true;

            // Set the new scene as active
            yield return new WaitUntil(() => loadOperation.isDone);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(newSceneName));
            _currentScene = newSceneName;

            // Fire the OnSceneLoaded event
            OnSceneLoaded?.Invoke(newSceneName);

            // Hide the loading screen if it was shown
            if (showLoadingScreen) {
                HideLoadingScreen();
            }

            // Unload the transition scene after loading is complete
            yield return StartCoroutine(UnloadSceneAsync(TransitionSceneName));
        }

        /// <summary>
        /// Coroutine to unload a scene by name.
        /// </summary>
        private IEnumerator UnloadSceneAsync(string sceneName) {
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(sceneName);

            // Wait until the scene is fully unloaded
            yield return new WaitUntil(() => unloadOperation.isDone);

            // Fire the OnSceneUnloaded event
            OnSceneUnloaded?.Invoke(sceneName);
        }

        /// <summary>
        /// Optional: Show loading screen UI.
        /// </summary>
        private void ShowLoadingScreen() {
            Debug.Log("Loading Screen Shown");
            // Implement loading screen logic here (e.g., enable a loading UI canvas)
        }

        /// <summary>
        /// Optional: Hide loading screen UI.
        /// </summary>
        private void HideLoadingScreen() {
            Debug.Log("Loading Screen Hidden");
            // Implement loading screen hide logic here (e.g., disable a loading UI canvas)
        }
    }
}