using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.UnitySingleton {

    public abstract class PersistentMonoSingleton<T> : MonoSingleton<T> where T : MonoSingleton<T> {

        protected override void Awake() {
            lock (lockObj) {
                if (instance == null) {
                    instance = this as T;
                    InitializeSingleton();
                    DontDestroyOnLoad(gameObject);
                } else if (instance != this) {
                    Debug.LogWarning($"[PersistentMonoSingleton] Duplicate instance of {typeof(T)} found. Destroying duplicate.");
                    Destroy(gameObject);
                    return;
                }
            }
        }

        protected override void OnApplicationQuit() {
            base.OnApplicationQuit();
            isQuitting = true;
        }

        protected virtual void ClearSceneReferences() { }

        private void OnEnable() {
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void OnDisable() {
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }

        private void OnSceneUnloaded(Scene scene) {
            ClearSceneReferences();
        }
    }
}