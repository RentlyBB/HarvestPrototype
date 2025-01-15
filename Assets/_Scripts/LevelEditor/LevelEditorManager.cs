using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.GameplayCore;
using _Scripts.GridCore;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.ScriptableObjects;
using _Scripts.UnitySingleton;
using _Scripts.Utilities;
using QFSW.QC;
using UnityEngine;
using UnityEngine.InputSystem;
using UnitySingleton;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace _Scripts.LevelEditor {
    public class LevelEditorManager : MonoSingleton<LevelEditorManager> {
        public LevelData levelToEdit;

        public TileTypeData selectedTileTypeData;
        public int selectedCountdownValue;
        public Vector2Int selectedStartingPosition = Vector2Int.zero;

        public bool setStartingPosition = false;

        public Transform playerMarker;

        private GameInput _gameInput;
        private Camera _cam;
        private Grid<TileGridObject> _grid;
        private TileTypeParser _tileTypeParser;

        protected override void Awake() {
            base.Awake();
            GameObject.FindWithTag("MainCamera").TryGetComponent(out _cam);

            TryGetComponent(out _tileTypeParser);

            _gameInput = new GameInput();
            _gameInput.Gameplay.MouseClick.performed += OnTileClick;
        }

        private void OnEnable() {
            _gameInput.Gameplay.Enable();
        }

        private void OnDisable() {
            _gameInput.Gameplay.MouseClick.performed -= OnTileClick;
            _gameInput.Gameplay.Disable();
        }

        private void Start() {
            CenterCamera();
        }

        private void CenterCamera() {
            if (_cam is not null) {
                _cam.transform.position = new Vector3((float)levelToEdit.gridWidth / 2, (float)levelToEdit.gridHeight / 2, -10);
            }
        }

        public void EditCurrentLevel() {
            InitGrid();
        }

        [Command]
        private void InitGrid() {
            if (_grid != null) {
                ClearGrid();
            }

            _grid = new Grid<TileGridObject>(levelToEdit.gridWidth, levelToEdit.gridHeight, 1, transform.position, (g, x, y) => new TileGridObject(g, x, y));

            // TileData - Holds data for only one tile in the grid 
            foreach (TileData tileData in levelToEdit.tiles) {
                if (tileData.tileTypeData is null) continue;

                _tileTypeParser.TileTypeToGameObject(tileData, _grid, out TileBase tileBase);
                if (tileBase is null) break;

                tileBase.gridPosition = tileData.gridPosition;
                _grid.GetGridDictionary()[tileData.gridPosition].SetTileBase(tileBase);

                tileBase.SetupTile();
                tileBase.transform.localScale = Vector3.one;
            }

            var pos = _grid.GetWorldPositionCellCenter(levelToEdit.startingGridPosition);
            playerMarker.position = new Vector3(pos.x, pos.y, -1);
        }

        private void OnTileClick(InputAction.CallbackContext ctx) {
            TileGridObject tileGridObject = _grid?.GetGridObject(Utils.GetMouseWorldPosition2D());

            // Check if player click on GridObject
            if (tileGridObject is null) return;

            // If true, stop function, because we do not want change any visual or data
            // we only want set starting position
            if (SetStartingPosition(tileGridObject)) return;

            //Find tileData in LevelData
            TileData tileData = FindTileData(tileGridObject.GetXY());

            //Edit data of the clicked tile
            // changes in tileData will be saved in LevelData
            EditDataInLevelData(tileData);

            //Re-InitGrid because i need to update the visual
            //This is not the effective way to do it, but it works and the levels are small, so I can use it here
            //And plus the editor will not be in the final game
            InitGrid();
        }

        // Set new player starting position
        private bool SetStartingPosition(TileGridObject tileGridObject) {
            if (!setStartingPosition) return false;

            levelToEdit.startingGridPosition = tileGridObject.GetXY();


            if (!playerMarker.gameObject.activeSelf) {
                playerMarker.gameObject.SetActive(true);
            }

            var pos = tileGridObject.GetWorldPositionCellCenter();
            playerMarker.position = new Vector3(pos.x, pos.y, -1);


            setStartingPosition = false;
            return true;
        }

        //tileData -> actual data of tile we clicked. In TileData are stored all information about tile
        //Editing actual level data, not a level editor visual data
        private void EditDataInLevelData(TileData tileData) {
            if (tileData is null) return;

            tileData.tileTypeData = selectedTileTypeData;

            if (selectedTileTypeData.tilePrefab.GetComponent<CountdownTileBase>()) {
                tileData.countdownValue = selectedCountdownValue;
            }
        }

        [Command]
        public void UpdateGridSize(int width, int height) {
            levelToEdit.SetGridSize(width, height);
            InitGrid();
            CenterCamera();
        }

        [Command]
        private void ClearGrid() {
            foreach (KeyValuePair<Vector2Int, TileGridObject> entry in _grid.GetGridDictionary()) {
                if (entry.Value.GetTile() is null) continue;

                Destroy(entry.Value.GetTile().gameObject);
            }
        }

        // Find TileData in LevelData storage
        private TileData FindTileData(Vector2Int position) {
            return levelToEdit.tiles.FirstOrDefault(tile => tile.gridPosition == position);
        }

        [Command]
        public void CreateNewLevel(string levelName, int gridWidth, int gridHeight) {
#if UNITY_EDITOR
            LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
            string path = "Assets/Data/Levels/" + levelName + ".asset";
            AssetDatabase.CreateAsset(levelData, path);

            levelToEdit = levelData;

            // Save the asset database and refresh the editor to reflect the changes
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("ScriptableObject created and saved at: " + path);

            levelData.SetGridSize(gridWidth, gridHeight);
#endif
        }

        public void SaveLevel() {
#if UNITY_EDITOR
            EditorUtility.SetDirty(levelToEdit); // Mark as dirty
            AssetDatabase.SaveAssets(); // Save all assets
            AssetDatabase.Refresh(); // Refresh asset database
            Debug.Log($"{levelToEdit.name} saved successfully!");
#else
            Debug.LogWarning("Saving ScriptableObjects is only supported in the Editor.");
#endif
        }
    }
}