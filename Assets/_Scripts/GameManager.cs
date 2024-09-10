using System;
using System.Collections.Generic;
using UnityEngine;
using _Scripts.Singleton;
using _Scripts.SOs;
using QFSW.QC;

namespace _Scripts {
    public class GameManager : Singleton<GameManager> {

        public PlayerBehaviour player;

        public List<GridLevelData> levels = new List<GridLevelData>();
        
        public int currentLevelGoal;

        [SerializeField]private int _currentLevelID = 0;
        private GridManager _gridManager;

        private void Start() {
            _gridManager = GridManager.Instance;
            LoadLevel(_currentLevelID);
        }

        private void Update() {
            if (currentLevelGoal == 0) {
                IncreasesCurrentLevelId();
                LoadLevel(_currentLevelID);
            }
        }

        private void IncreasesCurrentLevelId() {
            _currentLevelID++;
            if (_currentLevelID > levels.Count - 1) {
                _currentLevelID = 0;
            }
        }

        private void DecreasesCurrentLevelId() {
            _currentLevelID--;
            if (_currentLevelID < 0) {
                _currentLevelID = levels.Count - 1;
            }
        }


        [Command]
        public void LoadLevel(int levelID) {
            GridLevelData gridData = levels[levelID];
            if (_gridManager.LoadGridData(gridData)) {
                _currentLevelID = levelID;
                _gridManager.GenerateGrid();
                currentLevelGoal = gridData.goal;
                player.SetPosition((Vector2)gridData.playerStartingPosition);
            }
        }

        [Command]
        public void ResetLevel() {
            GridLevelData gridData = levels[_currentLevelID];
            if (_gridManager.LoadGridData(gridData)) {
                _gridManager.GenerateGrid();
                currentLevelGoal = gridData.goal;
                player.SetPosition(gridData.playerStartingPosition);

            }
        }

        [Command]
        public void NextLevel() { 
            IncreasesCurrentLevelId();
            ResetLevel();
        }

        [Command]
        public void PreviousLevel() {
            DecreasesCurrentLevelId();
            ResetLevel();
        }
    }
}