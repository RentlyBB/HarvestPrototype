using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.SOs;
using QFSW.QC;
using UnitySingleton;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.LevelEditor {
    public class GridManagerEditor : MonoSingleton<GridManagerEditor> {
        [SerializeField]
        private TileEditor tileEditorPrefab;

        private Dictionary<Vector2, TileEditor> _tiles;

        [SerializeField]
        private Transform _cam;

        public int width;
        public int height;

        public string valueToSave = "";

        public int levelGoal = 1;
        public Vector2Int levelPlayerStartingPosition;

        public GridLevelData loadedLevelInEditor;

        private void Awake() {
            if (_cam == null) {
                _cam = Camera.main?.transform;
            }
        }

        private void Start() {
            levelGoal = 1;
            valueToSave = "";
        }

        private void OnEnable() {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable() {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        [Command]
        public void GenerateGrid() {
            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }

            _tiles = new Dictionary<Vector2, TileEditor>();
            int i = 0;
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    var spawnedTile = Instantiate(tileEditorPrefab, new Vector3(x, y), Quaternion.identity);
                    spawnedTile.transform.SetParent(transform);
                    spawnedTile.name = $"Tile {x} {y}";

                    var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                    spawnedTile.gridPosition = new Vector2Int(x, y);

                    spawnedTile.Init(isOffset);
                    _tiles[new Vector2(x, y)] = spawnedTile;
                    i++;
                }
            }

            _cam.transform.position = new Vector3((float)width / 2 - 0.5f, (float)height / 2 - 0.5f, -10);
        }

        [Command]
        public void EditorLoadLevel(int levelId) {
            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }

            loadedLevelInEditor = GameManager.Instance.GetLevel(levelId);

            if (loadedLevelInEditor == null) return;

            levelGoal = loadedLevelInEditor.goal;

            width = loadedLevelInEditor.gridSize.x;
            height = loadedLevelInEditor.gridSize.y;

            levelPlayerStartingPosition = loadedLevelInEditor.playerStartingPosition;

            _tiles = new Dictionary<Vector2, TileEditor>();
            var i = 0;
            for (int x = 0; x < loadedLevelInEditor.gridSize.x; x++) {
                for (int y = 0; y < loadedLevelInEditor.gridSize.y; y++) {
                    var spawnedTile = Instantiate(tileEditorPrefab, new Vector3(x, y), Quaternion.identity);
                    spawnedTile.transform.SetParent(transform);
                    spawnedTile.name = $"Tile {x} {y}";

                    var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                    spawnedTile.gridPosition = new Vector2Int(x, y);

                    spawnedTile.Init(isOffset);

                    spawnedTile._editorTextValue = loadedLevelInEditor.tileData[i];
                    spawnedTile.TileUpdate();

                    _tiles[new Vector2(x, y)] = spawnedTile;
                    i++;
                }
            }

            _cam.transform.position = new Vector3((float)loadedLevelInEditor.gridSize.x / 2 - 0.5f, (float)loadedLevelInEditor.gridSize.y / 2 - 0.5f, -10);
        }

        public void SaveValueToTile(out string value) {
            value = valueToSave;
        }

        [Command]
        public void CreateAndSaveLevel(string levelName) {
            // Create an instance of the ScriptableObject
            GridLevelData gridLevelData = ScriptableObject.CreateInstance<GridLevelData>();

            // Add data to the ScriptableObject
            gridLevelData.goal = levelGoal;

            gridLevelData.gridSize = new Vector2Int(width, height);
            gridLevelData.playerStartingPosition = levelPlayerStartingPosition;

            var tileData = new List<string>();
            var arr = _tiles.Values.ToArray();

            for (var i = 0; i < arr.Length; i++) {
                tileData.Add(arr[i]._editorTextValue);
            }

            gridLevelData.tileData = tileData;
            // Save the ScriptableObject as an asset in the Assets folder
            string path = "Assets/Levels/" + levelName + ".asset";
            AssetDatabase.CreateAsset(gridLevelData, path);

            // Save the asset database and refresh the editor to reflect the changes
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("ScriptableObject created and saved at: " + path);
        }

        [Command]
        public void SaveLevel() {
            loadedLevelInEditor.goal = levelGoal;

            loadedLevelInEditor.gridSize = new Vector2Int(width, height);
            loadedLevelInEditor.playerStartingPosition = levelPlayerStartingPosition;

            var tileData = new List<string>();
            var arr = _tiles.Values.ToArray();

            for (var i = 0; i < arr.Length; i++) {
                tileData.Add(arr[i]._editorTextValue);
            }

            loadedLevelInEditor.tileData = tileData;

            EditorUtility.SetDirty(loadedLevelInEditor);

            // Save the changes to the asset
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
            EditorLoadLevel(GameManager.Instance.levelToBeEdited);
        }
    }
}