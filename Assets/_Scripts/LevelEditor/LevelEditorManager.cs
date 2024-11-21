using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts.GameplayCore;
using _Scripts.GridCore;
using _Scripts.TileCore.Enums;
using QFSW.QC;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnitySingleton;
using VHierarchy.Libs;
using TileData = _Scripts.GameplayCore.TileData;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace _Scripts.LevelEditor {

    public class LevelEditorManager : MonoSingleton<LevelEditorManager> {

        public LevelData levelToEdit;

        public TileType selectedTileType;
        public int selectedCountdownValue;
        public Vector2Int selectedStartingPosition = Vector2Int.zero;

        public bool setStartingPosition = false;

        private GameInput _gameInput;
        private Camera _cam;
        private Grid<EditorTileGridObject> _grid;

        private const string EditorTilePath = "TilePrefabs/EditorTile";

        protected override void Awake() {
            base.Awake();
            GameObject.FindWithTag("MainCamera").TryGetComponent(out _cam);
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
            if (_cam is not null) {
                _cam.transform.position = new Vector3((float)levelToEdit.gridWidth / 2, (float)levelToEdit.gridHeight / 2, -10);
            }

            EditCurrentLevel();
        }

        private void EditCurrentLevel() {
            InitGrid();
        }

        [Command]
        private void InitGrid() {
            if (_grid != null) {
                ClearGrid();
            }
            _grid = new Grid<EditorTileGridObject>(levelToEdit.gridWidth, levelToEdit.gridHeight, 1, transform.position, (g, x, y) => new EditorTileGridObject(g, x, y));

            // TileData - Holds data for only one tile in the grid 
            foreach (TileData tile in levelToEdit.tiles) {
                //Create a EditorTile
                EditorTile editorTile = Instantiate(Resources.Load<GameObject>(EditorTilePath), _grid.GetWorldPositionCellCenter(tile.gridPosition), Quaternion.identity, transform)
                    .GetComponent<EditorTile>();

                editorTile.tileType = tile.tileType;
                editorTile.tileVisualHandler.SetMainState(TileVisualParser.TileTypeToTileMainVisualState(tile.tileType));

                if (tile.tileType is TileType.CountdownTile or TileType.RepeatCountdownTile) {
                    editorTile.tileTextHandler.AddText(tile.countdownValue.ToString(), 42, Color.green);
                }

                _grid.GetGridDictionary()[tile.gridPosition].SetEditorTile(editorTile);
            }
        }

        private void OnTileClick(InputAction.CallbackContext ctx) {

            EditorTileGridObject editorTileGridObject = _grid?.GetGridObject(Utils.GetMouseWorldPosition2D());

            // Check if player click on GridObject
            if (editorTileGridObject is null) return;

            if (setStartingPosition) {
                levelToEdit.startingGridPosition = editorTileGridObject.GetXY();
                setStartingPosition = false;
                Debug.Log("Starting position is set to: " + editorTileGridObject.GetXY());
                return;
            }

            TileData tileData = FindTileData(editorTileGridObject.GetXY());

            //Edit data of the clicked tile
            EditData(tileData);

            //Change visual with text in the grid
            ChangeVisual(editorTileGridObject.GetTile(), tileData);
        }

        private void ChangeVisual(EditorTile editorTile, TileData tileData) {
            //Change visual of editorTile
            editorTile.tileVisualHandler.SetMainState(TileVisualParser.TileTypeToTileMainVisualState(selectedTileType));

            if (selectedTileType is not (TileType.CountdownTile or TileType.RepeatCountdownTile)) {
                editorTile.tileTextHandler.RemoveText();
                return;
            }

            if (editorTile.tileTextHandler.middleText is null) {
                editorTile.tileTextHandler.AddText("", 42, Color.green);
            }

            editorTile.tileTextHandler.UpdateText(tileData.countdownValue.ToString());
        }

        //tileData -> actual data of tile we clicked. In TileData are stored all information about tile
        private void EditData(TileData tileData) {
            if (tileData is null) return;

            tileData.tileType = selectedTileType;

            switch (selectedTileType) {
                case TileType.EmptyTile:
                case TileType.DefaultTile:
                    tileData.countdownValue = 0;
                    break;
                case TileType.CountdownTile:
                case TileType.RepeatCountdownTile:
                    tileData.countdownValue = selectedCountdownValue;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [Command]
        private void ClearGrid() {
            foreach (KeyValuePair<Vector2Int, EditorTileGridObject> entry in _grid.GetGridDictionary()) {
                Destroy(entry.Value.GetTile().gameObject);
            }
        }

        private TileData FindTileData(Vector2Int position) {
            return levelToEdit.tiles.FirstOrDefault(tile => tile.gridPosition == position);
        }

        [Command]
        public void CreateNewLevel(string levelName, int gridWidth, int gridHeight) {
            #if UNITY_EDITOR
            LevelData levelData = ScriptableObject.CreateInstance<LevelData>();
            string path = "Assets/Levels/" + levelName + ".asset";
            AssetDatabase.CreateAsset(levelData, path);

            levelToEdit = levelData;
            
            // Save the asset database and refresh the editor to reflect the changes
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("ScriptableObject created and saved at: " + path);

            levelData.gridWidth = gridWidth;
            levelData.gridHeight = gridHeight;
            levelData.UpdateTilesList();
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