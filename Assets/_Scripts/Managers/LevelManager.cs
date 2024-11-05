using System;
using _Scripts.GameplayCore;
using _Scripts.GridCore;
using _Scripts.PlayerCore;
using UnityEngine;
using UnitySingleton;

namespace _Scripts.Managers {
    public class LevelManager : PersistentMonoSingleton<LevelManager> {

        public LevelData levelToLoad;
        
        private GridHandler _gridHandler;
        private LevelLoader _levelLoader;

        private MovementHandler _playerMovementHandler;
        
        protected override void Awake() {
            base.Awake();
            TryGetComponent(out _gridHandler);
            TryGetComponent(out _levelLoader);
            GameObject.FindWithTag("Player").TryGetComponent(out _playerMovementHandler);
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
            
            _playerMovementHandler?.SetStartingTile(_gridHandler.GetGrid().GetGridObject(levelToLoad.startingGridPosition));
            
        }
    }
}