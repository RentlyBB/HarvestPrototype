using System;
using System.Collections.Generic;
using QFSW.QC;
using UnityEngine;

namespace _Scripts.GridCore {
    public class Grid<TGridObject> {
        
        public bool showDebug = false;

        public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;

        public class OnGridObjectChangedEventArgs : EventArgs {
            public int x;
            public int y;
        }

        private int width;
        private int height;
        private float cellSize;
        private Vector3 originPosition;
        private Dictionary<Vector2Int, TGridObject> gridDictionary;

        public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject) {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPosition = originPosition;

            gridDictionary = new Dictionary<Vector2Int, TGridObject>();
            
            for (int x = 0; x < width; x++) {
                for (int y = 0; y < height; y++) {
                    Vector2Int position = new Vector2Int(x, y);
                    gridDictionary[position] = createGridObject(this, x, y);
                }
            }

            if (showDebug) {
                TextMesh[,] debugTextArray = new TextMesh[width, height];
                
                for (int x = 0; x < width; x++) {
                    for (int y = 0; y < height; y++) {
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                        Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                        
                        //Text in middle of the cell
                        Utils.CreateTextWorld($"{x},{y}", GetWorldPositionCellCenter(x,y));
                    }
                }
            
                Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
            
                OnGridObjectChanged += (sender, eventArgs) => {
                    Vector2Int pos = new Vector2Int(eventArgs.x, eventArgs.y);
                    debugTextArray[eventArgs.x, eventArgs.y].text = gridDictionary.ContainsKey(pos) ? gridDictionary[pos]?.ToString() : "null";
                };
            }
        }

        public int GetWidth() {
            return width;
        }

        public int GetHeight() {
            return height;
        }

        public float GetCellSize() {
            return cellSize;
        }

        public Vector3 GetWorldPosition(int x, int y) {
            return new Vector3(x, y) * cellSize + originPosition;
        }
        
        public Vector3 GetWorldPositionCellCenter(int x, int y) {
            var value = cellSize / 2;
            var centerPosition = new Vector3(x, y) * cellSize + originPosition; ;
            return new Vector3(centerPosition.x + value, centerPosition.y + value, 0);
        }
        
        public Vector3 GetWorldPositionCellCenter(Vector2Int xy) {
           return GetWorldPositionCellCenter(xy.x, xy.y);
        }

        public void GetXY(Vector3 worldPosition, out int x, out int y) {
            x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
            y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        }

        public void SetGridObject(int x, int y, TGridObject value) {
            Vector2Int position = new Vector2Int(x, y);
            if (x >= 0 && y >= 0 && x < width && y < height) {
                gridDictionary[position] = value;
                TriggerGridObjectChanged(x, y);
            }
        }

        public void TriggerGridObjectChanged(int x, int y) {
            OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }
        
        public void SetGridObject(Vector3 worldPosition, TGridObject value) {
            GetXY(worldPosition, out int x, out int y);
            SetGridObject(x, y, value);
        }

        public TGridObject GetGridObject(int x, int y) {
            Vector2Int position = new Vector2Int(x, y);
            if (gridDictionary.ContainsKey(position)) {
                return gridDictionary[position];
            }
            return default;
        }

        public TGridObject GetGridObject(Vector3 worldPosition) {
            int x, y;
            GetXY(worldPosition, out x, out y);
            return GetGridObject(x, y);
        }

        public Dictionary<Vector2Int, TGridObject> GetGridDictionary() {
            return gridDictionary;
        }
    }
}