using System;
using _Scripts.GameplayCore;
using _Scripts.LevelEditor;
using _Scripts.UnitySingleton;
using QFSW.QC;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnitySingleton;

namespace _Scripts.Managers {
    public class EditorHandler : PersistentMonoSingleton<EditorHandler> {
        public LevelData levelToLoad;
        private const string EditorScene = "EditorScene";
        private const string GameScene = "RefactorScene";
        private bool _checkOnLoad = false;


        private void OnEnable() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        [Command]
        public void EditLevel() {
            _checkOnLoad = true;
            levelToLoad = GridManager.Instance.loadedLevelData;
            SceneManager.LoadScene(EditorScene);
        }


        [Command]
        public void TestLevel() {
            _checkOnLoad = true;
            levelToLoad = LevelEditorManager.Instance.levelToEdit;
            SceneManager.LoadScene(GameScene);
        }


        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode) {
            UpdateLoadedScene(scene);
        }

        
        //Check which scene is loaded and update its managers
        private void UpdateLoadedScene(Scene scene) {
            if(!_checkOnLoad) return;
            
            if (scene.name.Equals(EditorScene)) {
                var editor = LevelEditorManager.Instance;
                editor.levelToEdit = levelToLoad;
                editor.EditCurrentLevel();
            } else if (scene.name.Equals(GameScene)) {
                var gameplayManager = GameplayManager.Instance;
                gameplayManager.levelData = levelToLoad;
                gameplayManager.LoadLevel();
            }
            
            _checkOnLoad = false;
        }
    }
}