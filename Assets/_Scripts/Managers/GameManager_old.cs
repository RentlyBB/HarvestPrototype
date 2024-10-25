using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts.LevelEditor;
using UnityEngine;
using _Scripts.SOs;
using _Scripts.UI;
using QFSW.QC;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnitySingleton;

namespace _Scripts {
    public class GameManager_old : PersistentMonoSingleton<GameManager_old> {
        public PlayerBehaviour playerBehaviour;

        public Levels levelsInGame;

        public int currentLevelGoal;

        public int currentLevelID = 0;

        [HideInInspector]
        public int levelToBeEdited = 0;

        [HideInInspector]
        public bool editLevelInEditor = false;

        public TMP_Text text;

        private void Start() {
            SetLevelsIds();
            text.text = $"Level: {currentLevelID + 1} / {levelsInGame.levels.Count}";
        }

        public void SetLevelsIds() {
            for (int i = 0; i < levelsInGame.levels.Count; i++) {
                levelsInGame.levels[i].levelID = i;
            }
        }

        private void Update() {
            if (currentLevelGoal == 0) {
                IncreasesCurrentLevelId();
                playerBehaviour._nextTargetPosition = new List<Vector2Int>();
                LoadLevel(currentLevelID);
            }
        }

        private void OnEnable() {
            GameLevelBtn.GameControlEvent += GameLevelBtnOnGameControlEvent;
        }


        private void OnDisable() {
            GameLevelBtn.GameControlEvent -= GameLevelBtnOnGameControlEvent;
        }

        [Command]
        private void LevelList() {
            foreach (var level in levelsInGame.levels) {
                if (level.levelID == currentLevelID) {
                    Debug.Log("ID: " + level.levelID + ", Name: " + level.name + " – [Current Level]");
                } else {
                    Debug.Log("ID: " + level.levelID + ", Name: " + level.name);
                }
            }
        }

        private void IncreasesCurrentLevelId() {
            currentLevelID++;
            if (currentLevelID > levelsInGame.levels.Count - 1) {
                currentLevelID = 0;
            }
            text.text = $"Level: {currentLevelID + 1} / {levelsInGame.levels.Count}";
        }

        private void DecreasesCurrentLevelId() {
            currentLevelID--;
            if (currentLevelID < 0) {
                currentLevelID = levelsInGame.levels.Count - 1;
            }
            text.text = $"Level: {currentLevelID + 1} / {levelsInGame.levels.Count}";
        }


        [Command]
        public void LoadLevel(int levelID) {
            GridLevelData gridData = levelsInGame.levels[levelID];
            if (GridManager_old.Instance.LoadGridData(gridData)) {
                currentLevelID = levelID;
                GridManager_old.Instance.GenerateGrid();
                currentLevelGoal = gridData.goal;
                SetPlayerPosition(gridData);
                playerBehaviour.ResetGhost();
                Debug.Log("Loaded level: ID: " + gridData.levelID + ", Name: " + gridData.name);
            }
        }

        [Command]
        public void LoadLevel() {
            GridLevelData gridData = levelsInGame.levels[currentLevelID];
            if (GridManager_old.Instance.LoadGridData(gridData)) {
                GridManager_old.Instance.GenerateGrid();
                currentLevelGoal = gridData.goal;
                playerBehaviour.ResetGhost();
                SetPlayerPosition();
            }
        }

        public void SetPlayerPosition(GridLevelData gridData) {
            if (!playerBehaviour) {
                playerBehaviour = FindObjectOfType<PlayerBehaviour>();
            }

            playerBehaviour.SetPosition((Vector2)gridData.playerStartingPosition);
        }

        public void SetPlayerPosition() {
            GridLevelData gridData = levelsInGame.levels[currentLevelID];
            if (playerBehaviour == null) {
                playerBehaviour = FindObjectOfType<PlayerBehaviour>();
            }

            playerBehaviour.SetPosition((Vector2)gridData.playerStartingPosition);
        }

        [Command]
        public void ResetLevel() {
            GridLevelData gridData = levelsInGame.levels[currentLevelID];
            if (GridManager_old.Instance.LoadGridData(gridData)) {
                GridManager_old.Instance.GenerateGrid();
                currentLevelGoal = gridData.goal;
                playerBehaviour.SetPosition(gridData.playerStartingPosition);
                playerBehaviour._nextTargetPosition = new List<Vector2Int>();
                playerBehaviour._waitingTargetPosition = new List<Vector2Int>();
                playerBehaviour.isBeingPushed = false;
                playerBehaviour.ResetGhost();
                Debug.Log("Current level: " + gridData.name);
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
            if (i >= levelsInGame.levels.Count) {
                Debug.LogWarning("Level number:" + i + " is not exist in GameManager level list");
                return null;
            }

            return levelsInGame.levels[i];
        }

        public GridLevelData GetLevel() {
            return levelsInGame.levels[currentLevelID];
        }

#if UNITY_EDITOR

        //Edit current level
        [Command]
        public void EditLevel() {
            levelToBeEdited = currentLevelID;
            editLevelInEditor = true;
            ChangeScene("EditorScene");
        }


        //Edit specific level
        [Command]
        public void EditLevel(int i) {
            levelToBeEdited = i;
            editLevelInEditor = true;
            ChangeScene("EditorScene");
        }

        [Command]
        public void TestLevel() {
            GridManagerEditor.Instance.SaveLevel();
            currentLevelID = GridManagerEditor.Instance.loadedLevelInEditor.levelID;
            currentLevelGoal = GridManagerEditor.Instance.loadedLevelInEditor.goal;

            ChangeScene("GameScene");
        }

        [Command]
        private void OpenEditor() {
            editLevelInEditor = false;
            ChangeScene("EditorScene");
        }
#endif

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

            playerBehaviour = FindObjectOfType<PlayerBehaviour>();

            Debug.Log("Scene fully loaded and activated.");
        }

        private void GameLevelBtnOnGameControlEvent(int controlId) {
            if (controlId == 0) {
                PreviousLevel();
            } else if (controlId == 1) {
                ResetLevel();
            } else if (controlId == 2) {
                NextLevel();
            }
        }
    }
}