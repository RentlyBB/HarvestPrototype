using _Scripts.GameplayCore;
using _Scripts.GridCore;
using EditorScripts;
using UnityEngine;
using UnitySingleton;

namespace _Scripts.Managers {
    public class LevelManager : PersistentMonoSingleton<LevelManager> {

        public LevelData levelToLoad;
        
        private GridHandler _gridHandler;
        private LevelLoader _levelLoader;
        protected override void Awake() {
            base.Awake();
            TryGetComponent(out _gridHandler);
            TryGetComponent(out _levelLoader);
        }

        [InvokeButton]
        public void LoadLevel() {
            if (!_gridHandler.InitGridLevel(levelToLoad)) {
                Debug.LogError("Not able to init the grid");   
                return;
            }
            
            _levelLoader.LoadLevel(_gridHandler.GetGrid(), levelToLoad);
        }

    }
}