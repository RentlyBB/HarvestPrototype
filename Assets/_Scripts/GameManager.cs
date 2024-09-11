using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.LevelEditor;
using UnityEngine;
using _Scripts.Singleton;
using _Scripts.SOs;
using QFSW.QC;
using UnityEngine.SceneManagement;

namespace _Scripts {
    public class GameManager : Singleton<GameManager> {

        public PlayerBehaviour player;

        public List<GridLevelData> levels = new List<GridLevelData>();
        
        public int currentLevelGoal;

        [SerializeField]private int _currentLevelID = 0;
        private GridManager _gridManager;

        private void Start() {
            _gridManager = GridManager.Instance;

            //LoadLevel(_currentLevelID);
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
        public void LoadLevel() {
            GridLevelData gridData = levels[_currentLevelID];
            if (_gridManager.LoadGridData(gridData)) {
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

        public GridLevelData GetLevel(int i) {
            if (i >= levels.Count) {
                Debug.LogWarning("Level number:" + i + " is not exist in GameManager level list");
                return null;
            }

            return levels[i];
        }

        //Edit current level
        [Command]
        public void EditLevel() {
            ChangeScene("EditorScene");
            Debug.Log(GridManagerEditor.Instance.name);
            
            //GridManagerEditor.Instance.EditorLoadLevel(_currentLevelID);
        }
        

        //Edit specific level
        [Command]
        public void EditLevel(int i) {
            Debug.Log(GridManagerEditor.Instance.name);
            //Debug.Log("Edit specific level is not implemented yet");
        }
        
        
        public void ChangeScene(string sceneName)
        {
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        private IEnumerator LoadSceneAsync(string sceneName)
        {
            // Begin to load the Scene asynchronously
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false;  // Prevent the scene from activating immediately

            // Optionally, you can display a loading screen or progress bar here

            while (!asyncOperation.isDone)
            {
                // Output loading progress (for a progress bar or debug)
                float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);  // Normalize to 0-1 range
                Debug.Log($"Loading progress: {progress * 100}%");

                // Check if the scene has finished loading (i.e., it reaches 90% progress)
                if (asyncOperation.progress >= 0.9f)
                {
                    // Optional: You can wait for user input or a specific event before continuing
                    // Debug.Log("Press any key to activate the scene");
                    // if (Input.anyKeyDown)
                    // {
                    asyncOperation.allowSceneActivation = true;  // Activate the scene
                    // }
                }

                // Yield the frame and continue the loop until the scene is fully loaded and activated
                yield return null;
            }

            Debug.Log("Scene fully loaded and activated.");
        }
    }
}