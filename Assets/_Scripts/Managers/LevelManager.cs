using System;
using _Scripts.GameplayCore;
using _Scripts.GridCore;
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

        private void Start() {
            LoadLevel();
        }

        public void LoadLevel() {
            if (!_gridHandler.InitGrid(levelToLoad)) {
                Debug.LogError("Not able to init the grid");   
                return;
            }
            
            _levelLoader.LoadLevel(_gridHandler.GetGrid(), levelToLoad);
        }
    }
}