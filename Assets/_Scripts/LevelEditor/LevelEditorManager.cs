using _Scripts.GameplayCore;
using _Scripts.GridCore;
using _Scripts.TileCore.Enums;
using UnityEngine;
using UnitySingleton;

namespace _Scripts.LevelEditor {
    public class LevelEditorManager : MonoSingleton<LevelEditorManager> {
        public LevelData levelDataToEdit;

        private Camera _cam;

        private Grid<EditorTileGridObject> _grid;

        private const string EditorTilePath = "TilePrefabs/EditorTile";

        protected override void Awake() {
            base.Awake();
            GameObject.FindWithTag("MainCamera").TryGetComponent(out _cam);
        }

        private void Start() {
            if (_cam is not null) {
                _cam.transform.position = new Vector3((float)levelDataToEdit.gridWidth / 2, (float)levelDataToEdit.gridHeight / 2, -10);
            }


            EditCurentLevel();
        }

        private void EditCurentLevel() {
            InitGrid();
        }

        private void InitGrid() {
            if (_grid != null) {
                // TODO: Clear the grid
            }
            _grid = new Grid<EditorTileGridObject>(levelDataToEdit.gridWidth, levelDataToEdit.gridHeight, 1, transform.position, (g, x, y) => new EditorTileGridObject(g, x, y));

            FillGrid();
        }

        private void FillGrid() {
            // TileData - Holds data for only one tile in the grid 
            foreach (TileData tile in levelDataToEdit.tiles) {
                //Create a EditorTile
                var editorTile = Instantiate(Resources.Load<GameObject>(EditorTilePath), _grid.GetWorldPositionCellCenter(tile.gridPosition), Quaternion.identity, transform)
                    .GetComponent<EditorTile>();

                editorTile.tileType = tile.tileType;
                editorTile.tileVisualHandler.SetMainState(TileVisualParser.TileTypeToTileMainVisualState(tile.tileType));

                if (tile.tileType is TileType.CountdownTile or TileType.RepeatCountdownTile) {
                    editorTile.tileTextHandler.AddText(tile.countdownValue.ToString(), 42, Color.green);
                }

                _grid.GetGridDictionary()[tile.gridPosition].SetEditorTile(editorTile);
            }
        }

        private void ClearGrid() {
            if(_grid is null) return;
            
            
            
            
        }
    }
}