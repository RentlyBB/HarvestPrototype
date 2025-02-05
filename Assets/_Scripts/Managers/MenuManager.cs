using System;
using _Scripts.GameplayCore;
using _Scripts.UI;
using _Scripts.UnitySingleton;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Managers {
    public class MenuManager : MonoSingleton<MenuManager> {
    
        public GameObject btnWorldPrefab;
        public GameObject worldSelectionGrid;
        
        public GameObject btnLevelPrefab;
        public GameObject levelSelectionGrid;

        public int worldSelected = 0;
        
        public GameData gameData;

        protected override void Awake() {
            base.Awake();
            gameData = GameDataHandler.Instance.LoadGameData();
        }
        
        private void Start() {
            CreateWorldButtons();
            CreateLevelButtons();
        }

        public void CreateWorldButtons() {
            foreach (var world in gameData.worlds) {
                Instantiate(btnWorldPrefab, worldSelectionGrid.transform);
            }
        }

        public void CreateLevelButtons() {
            foreach (var level in gameData.worlds[worldSelected].levels) {
                var btn = Instantiate(btnLevelPrefab, levelSelectionGrid.transform);
                btn.GetComponent<LevelButton>().btnLevelID = level.levelID;
            }
        }
    }
}