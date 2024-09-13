using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts {
    public class Tile : MonoBehaviour {
        public static event UnityAction<Vector2Int> PlayerMove = delegate { };

        [SerializeField]
        private Color _baseColor, _offsetColor, _harvestedColor, _badHarvestColor, _freezedColor, _exclamationColor;

        [SerializeField]
        private SpriteRenderer _renderer;

        [SerializeField]
        private GameObject _highlight;

        public Vector2Int gridPosition;
        public int tileValue;
        private int _defaultTileValue;
        public bool moveable;

        private bool _mouseOnTile = false;
        private TextMesh _middleText;
        private TextMesh _leftCornerText;
        private string _textToShow;
        private bool _harvestable = false;
        private bool _decreaseValue = true;
        private bool _isFreeze = false;
        private TileType _tileType;
        private bool _isOffset;
        private TileState _tileState;

        private void OnEnable() {
            PlayerBehaviour.DegreaseTileValueEvent += DecreaseValue;
        }

        private void OnDisable() {
            PlayerBehaviour.DegreaseTileValueEvent -= DecreaseValue;
        }

        public void Init(bool isOffset, string gridLevelData) {
            _isOffset = isOffset;
            ChangeTileColor();

            EvaluateLevelData(gridLevelData);
            _middleText = Utils.CreateTextWorld($"{_textToShow}", transform.position, 32, transform, Color.white);
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
                    _middleText.text = $"{_textToShow}";
                    break;
                case TileType.ExclamationTile:
                    _middleText.text = $"!{_textToShow}";
                    break;
            }
        }

        private void ChangeTileColor() {
            if (_tileState == TileState.Freeze) {
                _renderer.color = _freezedColor;
            }else if (_tileState == TileState.Normal) {
                _renderer.color = _isOffset ? _offsetColor : _baseColor;
            }else if (_tileState == TileState.BadHarvested) {
                _renderer.color = _badHarvestColor;
            }else if (_tileState == TileState.GoodHarvested) {
                _renderer.color = _harvestedColor;
            } else if (_tileState == TileState.Invisible) {
                _renderer.color = new Color(0, 0, 0, 0);
            }else if (_tileState == TileState.Exclamation) {
                _renderer.color = _exclamationColor;
            }
        }

        private void EvaluateLevelData(string data) {
            if (data.Contains("X")) {
                moveable = false;
                _textToShow = "";
                _tileType = TileType.NotMoveable;
                _tileState = TileState.Invisible;
            } else if (data.Contains("M")) {
                moveable = true;
                _textToShow = "";
                _tileType = TileType.Moveable;
                _tileState = TileState.Normal;
            } else if (data.Contains("N")) {
                data = data.Replace("N", string.Empty);
                moveable = true;
                tileValue = Int32.Parse(data);
                _harvestable = true;
                _defaultTileValue = tileValue;
                _textToShow = data;
                _tileType = TileType.ClassicTile;
                _tileState = TileState.Normal;
            } else if (data.Contains("!")) {
                data = data.Replace("!", string.Empty);
                _tileState = TileState.Exclamation;
                moveable = true;
                tileValue = Int32.Parse(data);
                _harvestable = false;
                _leftCornerText = Utils.CreateTextWorld($"{tileValue}", transform.position + new Vector3(0.35f, -0.35f), 24, transform, Color.white);
                _defaultTileValue = tileValue;
                _textToShow = $"!{tileValue}";
                _tileType = TileType.ExclamationTile;
            } else if (data.Contains("F")) {
                _tileState = TileState.Freeze;
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

            ChangeTileColor();
        }

        public bool Harvest() {
            if (_harvestable) {
                if (tileValue == 0) {
                    //Good
                    GameManager.Instance.currentLevelGoal -= 1;
                    _tileState = TileState.GoodHarvested;
                } else {
                    //Bad
                    _tileState = TileState.BadHarvested;
                }
                ChangeTileColor();
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
                if(_tileState == TileState.Freeze) {
                    _tileState = TileState.Normal;
                    ChangeTileColor();
                }
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
                    _tileState = TileState.BadHarvested;
                }

                TextUpdate("");
                Harvest();
                ChangeTileColor();
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
                    _tileState = TileState.Normal;
                    _tileType = TileType.ClassicTile;
                    _leftCornerText.text = "";
                    tileValue = _defaultTileValue + 1;
                    ChangeTileColor();
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
                // Using GetRow method to get all tiles in row
                GridManager.Instance.GetAllInRow(gridPosition.y, out var allTiles);
                foreach (var tile in allTiles) {
                    tile._isFreeze = true;
                    if (tile._tileState == TileState.Normal) {
                        tile._tileState = TileState.Freeze;
                        tile.ChangeTileColor();
                    }
                }
            } else if (_tileType == TileType.FreezeTileVertical) {
                // Using GetColumn method to get all tiles in column
                GridManager.Instance.GetAllInColumn(gridPosition.x, out var allTiles);
                foreach (var tile in allTiles) {
                    tile._isFreeze = true;
                    if (tile._tileState == TileState.Normal) {
                        tile._tileState = TileState.Freeze;
                        tile.ChangeTileColor();
                    }
                }
            }
            
            
        }
    }
}