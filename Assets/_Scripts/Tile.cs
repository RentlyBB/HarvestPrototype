using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts {
    public class Tile : MonoBehaviour {
        public static event UnityAction<Vector2Int> PlayerMove = delegate { };

        [SerializeField]
        private Color _baseColor, _offsetColor, _harvestedColor, _badHarvestColor;

        [SerializeField]
        private SpriteRenderer _renderer;

        [SerializeField]
        private GameObject _highlight;

        public Vector2Int gridPosition;
        public int tileValue;
        private int _defaultTileValue;
        public bool moveable;

        private bool _mouseOnTile = false;
        private TextMesh _textMesh;
        private string _textToShow;
        private bool _harvestable = false;
        private bool _decreaseValue = true;
        private bool _isFreeze = false;
        private TileType _tileType;

        private void OnEnable() {
            PlayerBehaviour.DegreaseTileValueEvent += DecreaseValue;
        }

        private void OnDisable() {
            PlayerBehaviour.DegreaseTileValueEvent -= DecreaseValue;
        }

        public void Init(bool isOffset, string gridLevelData) {
            _renderer.color = isOffset ? _offsetColor : _baseColor;

            EvaluateLevelData(gridLevelData);
            //_textMesh = Utils.CreateTextWorld($"{gridPosition.x},{gridPosition.y}", transform.position, 32, transform, Color.white);
            _textMesh = Utils.CreateTextWorld($"{_textToShow}", transform.position, 32, transform, Color.white);
        }

        void OnMouseEnter() {
            _highlight.SetActive(true);
            _mouseOnTile = true;
        }

        void OnMouseExit() {
            _highlight.SetActive(false);
            _mouseOnTile = false;
        }

        private void OnMouseDown() {
            if (_mouseOnTile && moveable) {
                PlayerMove?.Invoke(gridPosition);
            }
        }

        private void TextUpdate(string text) {
            _textToShow = text;

            switch (_tileType) {
                case TileType.ClassicTile:
                    _textMesh.text = $"{_textToShow}";
                    break;
                case TileType.ExclamationTile:
                    _textMesh.text = $"!{_textToShow}";
                    break;
            }
        }

        private void EvaluateLevelData(string data) {
            if (data.Contains("X")) {
                moveable = false;
                _textToShow = "X";
                _tileType = TileType.ClassicTile;
            } else if (data.Contains("M")) {
                moveable = true;
                _textToShow = "";
                _tileType = TileType.ClassicTile;
            } else if (data.Contains("N")) {
                data = data.Replace("N", string.Empty);
                moveable = true;
                tileValue = Int32.Parse(data);
                _harvestable = true;
                _defaultTileValue = tileValue;
                _textToShow = data;
                _tileType = TileType.ClassicTile;
            } else if (data.Contains("!")) {
                data = data.Replace("!", string.Empty);
                moveable = true;
                tileValue = Int32.Parse(data);
                _harvestable = false;
                _defaultTileValue = tileValue;
                _textToShow = $"!{tileValue}";
                _tileType = TileType.ExclamationTile;
            } else if (data.Contains("F")) {
                if (data.Contains("H")) {
                    _tileType = TileType.FreezeTileHorizontal;
                } else if (data.Contains("V")) {
                    _tileType = TileType.FreezeTileVertical;
                }

                data = data.Replace("F", "*");
                _decreaseValue = false;
                moveable = true;
                _textToShow = data;
            }
        }

        public bool Harvest() {
            if (_harvestable) {
                if (tileValue == 0) {
                    //Good
                    GameManager.Instance.currentLevelGoal -= 1;
                    _renderer.color = _harvestedColor;
                } else {
                    //Bad
                    _renderer.color = _badHarvestColor;
                }

                TextUpdate("");
                _harvestable = false;
            }

            return true;
        }

        private void DecreaseValue() {
            if (!_decreaseValue) {
                return;
            }

            if (_isFreeze) {
                _isFreeze = false;
                return;
            }


            tileValue--;

            //Refresh text
            if (_textToShow.Contains("X") || _textToShow == "" || _textToShow.Length == 0) {
                return;
            }

            if (tileValue < 0) {
                if (_tileType == TileType.ExclamationTile) {
                    _tileType = TileType.ClassicTile;
                    _renderer.color = _badHarvestColor;
                }

                TextUpdate("");
                Harvest();
            } else {
                TextUpdate(tileValue.ToString());
            }
        }

        public void OnTileStep() {
            switch (_tileType) {
                case TileType.ClassicTile:
                    Harvest();
                    break;
                case TileType.ExclamationTile:
                    _harvestable = true;
                    _tileType = TileType.ClassicTile;
                    tileValue = _defaultTileValue + 1;
                    break;
            }
        }

        public void OnTileStepAfter() {
            switch (_tileType) {
                case TileType.FreezeTileHorizontal:
                case TileType.FreezeTileVertical:
                    FreezeLine();
                    break;
            }
        }


        private void FreezeLine() {
            
            if (_tileType == TileType.FreezeTileHorizontal) {
                GridManager.Instance.GetAllInRow(gridPosition.y, out var allTiles);
                foreach (var tile in allTiles) {
                    tile._isFreeze = true;
                }
                
            } else if (_tileType == TileType.FreezeTileVertical) {
                GridManager.Instance.GetAllInColumn(gridPosition.x, out var allTiles);
                foreach (var tile in allTiles) {
                    tile._isFreeze = true;
                }
            }
        }
    }
}