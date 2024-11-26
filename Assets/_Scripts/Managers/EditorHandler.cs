using System;
using _Scripts.GameplayCore;
using _Scripts.LevelEditor;
using QFSW.QC;
using UnityEngine.SceneManagement;
using UnitySingleton;

namespace _Scripts.Managers {
    public class EditorHandler : PersistentMonoSingleton<EditorHandler> {
        private LevelData _levelToLoad;
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
            _levelToLoad = GridManager.Instance.currentLevelData;
            SceneManager.LoadScene(EditorScene);
        }


        [Command]
        public void TestLevel() {
            _checkOnLoad = true;
            _levelToLoad = LevelEditorManager.Instance.levelToEdit;
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
                editor.levelToEdit = _levelToLoad;
                editor.EditCurrentLevel();
            } else if (scene.name.Equals(GameScene)) {
                var grid = GridManager.Instance;
                grid.currentLevelData = _levelToLoad;
                grid.LoadLevel();
            }
            
            _checkOnLoad = false;
        }
    }
}