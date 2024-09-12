using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.LevelEditor;
using UnityEngine;
using _Scripts.SOs;
using _Scripts.UI;
using QFSW.QC;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnitySingleton;

namespace _Scripts {
    public class GameManager : PersistentMonoSingleton<GameManager> {
        public PlayerBehaviour player;

        public List<GridLevelData> levels = new List<GridLevelData>();

        public int currentLevelGoal;

        public int currentLevelID = 0;

        [HideInInspector]
        public int levelToBeEdited = 0;
        
        private void Start() {

            for (int i = 0; i < levels.Count; i++) {
                levels[i].levelID = i;
            }
        }

        private void Update() {
            if (currentLevelGoal == 0) {
                IncreasesCurrentLevelId();
                LoadLevel(currentLevelID);
            }
        }

        private void OnEnable() {
            GameLevelBtn.GameControlEvent += GameLevelBtnOnGameControlEvent;
        }
        
       
        private void OnDisable() {
            GameLevelBtn.GameControlEvent -= GameLevelBtnOnGameControlEvent;
        }

        private void IncreasesCurrentLevelId() {
            currentLevelID++;
            if (currentLevelID > levels.Count - 1) {
                currentLevelID = 0;
            }
        }

        private void DecreasesCurrentLevelId() {
            currentLevelID--;
            if (currentLevelID < 0) {
                currentLevelID = levels.Count - 1;
            }
        }


        [Command]
        public void LoadLevel(int levelID) {
            GridLevelData gridData = levels[levelID];
            if (GridManager.Instance.LoadGridData(gridData)) {
                currentLevelID = levelID;
                GridManager.Instance.GenerateGrid();
                currentLevelGoal = gridData.goal;
                SetPlayerPosition(gridData);
            }
        }

        [Command]
        public void LoadLevel() {
            GridLevelData gridData = levels[currentLevelID];
            if (GridManager.Instance.LoadGridData(gridData)) {
                GridManager.Instance.GenerateGrid();
                currentLevelGoal = gridData.goal;
                SetPlayerPosition();
            }
        }

        public void SetPlayerPosition(GridLevelData gridData) {
            if (player == null) {
                player = FindObjectOfType<PlayerBehaviour>();
            }
            player.SetPosition((Vector2)gridData.playerStartingPosition);
        }
        
        public void SetPlayerPosition() {
            GridLevelData gridData = levels[currentLevelID];
            if (player == null) {
                player = FindObjectOfType<PlayerBehaviour>();
            }
            player.SetPosition((Vector2)gridData.playerStartingPosition);
        }

        [Command]
        public void ResetLevel() {
            GridLevelData gridData = levels[currentLevelID];
            if (GridManager.Instance.LoadGridData(gridData)) {
                GridManager.Instance.GenerateGrid();
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
        
        public GridLevelData GetLevel() {
            return levels[currentLevelID];
        }

        //Edit current level
        [Command]
        public void EditLevel() {
            levelToBeEdited = currentLevelID;
            ChangeScene("EditorScene");
        }


        //Edit specific level
        [Command]
        public void EditLevel(int i) {
            levelToBeEdited = i;
            ChangeScene("EditorScene");
        }
        
        [Command]
        public void TestLevel() { 
            GridManagerEditor.Instance.SaveLevel();
            currentLevelID = GridManagerEditor.Instance.loadedLevelInEditor.levelID;
            currentLevelGoal = GridManagerEditor.Instance.loadedLevelInEditor.goal;
            
            ChangeScene("GameScene");
        }

        public void ChangeScene(string sceneName) {
            StartCoroutine(LoadSceneAsync(sceneName));
        }

        private IEnumerator LoadSceneAsync(string sceneName) {
            // Begin to load the Scene asynchronously
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
            asyncOperation.allowSceneActivation = false; // Prevent the scene from activating immediately

            // Optionally, you can display a loading screen or progress bar here

            while (!asyncOperation.isDone) {
                // Output loading progress (for a progress bar or debug)
                float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f); // Normalize to 0-1 range
                Debug.Log($"Loading progress: {progress * 100}%");

                // Check if the scene has finished loading (i.e., it reaches 90% progress)
                if (asyncOperation.progress >= 0.9f) {
                    // Optional: You can wait for user input or a specific event before continuing
                    // Debug.Log("Press any key to activate the scene");
                    // if (Input.anyKeyDown) {
                        asyncOperation.allowSceneActivation = true; // Activate the scene
                    // }
                }

                // Yield the frame and continue the loop until the scene is fully loaded and activated
                yield return null;
            }

            player = FindObjectOfType<PlayerBehaviour>();
            
            Debug.Log("Scene fully loaded and activated.");
        }
        
        private void GameLevelBtnOnGameControlEvent(int controlId) {
            if (controlId == 0) {
                PreviousLevel();
            }else if (controlId == 1) {
                ResetLevel();
            }else if (controlId == 2) {
                NextLevel();
            }
        }

    }
}