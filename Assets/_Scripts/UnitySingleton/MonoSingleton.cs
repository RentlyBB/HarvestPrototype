using UnityEngine;
using UnitySingleton;

namespace _Scripts.UnitySingleton {

    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T> {

        protected static T instance;
        protected static readonly object lockObj = new object();
        protected static bool isQuitting = false;

        private SingletonInitializationStatus initializationStatus = SingletonInitializationStatus.None;

        public static T Instance {
            get {
                if (isQuitting) {
                    Debug.LogWarning($"[MonoSingleton] Instance of {typeof(T)} requested after application is quitting. Returning null.");
                    return null;
                }

                lock (lockObj) {
                    if (instance == null) {
                        instance = FindFirstObjectByType<T>();
                        if (instance == null) {
                            GameObject obj = new GameObject(typeof(T).Name);
                            instance = obj.AddComponent<T>();
                            instance.OnMonoSingletonCreated();
                        }
                    }
                    return instance;
                }
            }
        }

        public virtual bool IsInitialized => initializationStatus == SingletonInitializationStatus.Initialized;

        protected virtual void Awake() {
            lock (lockObj) {
                if (instance == null) {
                    instance = this as T;
                    InitializeSingleton();
                } else if (instance != this) {
                    Debug.LogWarning($"[MonoSingleton] Duplicate instance of {typeof(T)} found. Destroying duplicate.");
                    Destroy(gameObject);
                    return;
                }
            }
        }

        protected virtual void OnApplicationQuit() {
            isQuitting = true;
        }

        protected virtual void OnMonoSingletonCreated() { }

        protected virtual void OnInitializing() { }

        protected virtual void OnInitialized() { }

        public virtual void InitializeSingleton() {
            if (initializationStatus != SingletonInitializationStatus.None) {
                return;
            }

            initializationStatus = SingletonInitializationStatus.Initializing;
            OnInitializing();
            initializationStatus = SingletonInitializationStatus.Initialized;
            OnInitialized();
        }

        public virtual void ClearSingleton() { }

        public static void DestroyInstance() {
            if (instance == null) {
                return;
            }

            instance.ClearSingleton();
            Destroy(instance.gameObject);
            instance = null;
        }

        public static void ResetSingleton() {
            DestroyInstance();
        }
    }
}