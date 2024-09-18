using System;
using System.Collections.Generic;
using Enums;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts {
    public class Tile : MonoBehaviour {
        public static event UnityAction<Vector2Int> PlayerMove = delegate { };
        public static event UnityAction<Vector2Int> PushPlayer = delegate { };

        [SerializeField]
        private Color _baseColor, _offsetColor, _harvestedColor, _badHarvestColor, _freezedColor, _exclamationColor;
        
        [SerializeField]
        private Sprite _sprDefaultTile, _sprGoodHarvestTile, _sprBadHarvestTile, _sprFreezeTile, _sprExclamationTile;

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
        public TileType _tileType;
        private bool _isOffset;
        public TileState _tileState;
        private bool _canBreak = false;
        private bool _breakable = false;
        private SpriteRenderer _tileImg;

        private void OnEnable() {
            PlayerBehaviour.DecreaseTileValueEvent += DecreaseValue;
        }

        private void OnDisable() {
            PlayerBehaviour.DecreaseTileValueEvent -= DecreaseValue;
        }

        public void Init(bool isOffset, string gridLevelData) {
            _isOffset = isOffset;
            ChangeTileColor();

            EvaluateLevelData(gridLevelData);
            _middleText = Utils.CreateTextWorld($"{_textToShow}", transform.position, 32, transform, Color.white);
        }

        void OnMouseEnter() {
            if(_tileType == TileType.NotMoveable) return;
            _highlight.SetActive(true);
            _mouseOnTile = true;
        }

        void OnMouseExit() {
            if(_tileType == TileType.NotMoveable) return;
            _highlight.SetActive(false);
            _mouseOnTile = false;
        }

        private void OnMouseDown() {
            if(_tileType == TileType.NotMoveable) return;
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
                _renderer.sprite = _sprFreezeTile;
            }else if (_tileState == TileState.Normal) {
                _renderer.sprite = _sprDefaultTile;
            }else if (_tileState == TileState.BadHarvested) {
                _renderer.sprite = _sprBadHarvestTile;
            }else if (_tileState == TileState.GoodHarvested) {
                _renderer.sprite = _sprGoodHarvestTile;
            } else if (_tileState == TileState.Invisible) {
                _renderer.sprite = null;
            }else if (_tileState == TileState.Exclamation) {
                _renderer.sprite = _sprExclamationTile;
            }
        }

        private void EvaluateLevelData(string data) {

            if (data.Contains("*")) {
                //Breakable tile – breaks after player leaves the tile    
                data = data.Replace("*", string.Empty);
                _breakable = true;
            }

            if (data.Contains("X")) {
                moveable = false;
                _textToShow = "";
                _tileType = TileType.NotMoveable;
                _tileState = TileState.Invisible;
            } else if (data.Contains("M") || string.IsNullOrWhiteSpace(data)) {
                moveable = true;
                _textToShow = "";
                _tileType = TileType.Moveable;
                _tileState = TileState.Normal;
            } else if (data.Contains("N")) {
                data = data.Replace("N", string.Empty);
                moveable = true;
                tileValue = Int32.Parse(data);
                _harvestable = true;
                if (tileValue == 0) {
                    _tileImg = Utils.CreateSpriteWorld(Resources.Load<Sprite>("icon_outline_checkmark"), transform.position + new Vector3(0, 0, -1), new Vector2(0.5f,0.5f), this.transform);
                    _textToShow = "";
                } else {
                    _textToShow = data;
                }
                _defaultTileValue = tileValue;
                _tileType = TileType.ClassicTile;
                _tileState = TileState.Normal;
            } else if (data.Contains("!")) {
                data = data.Replace("!", string.Empty);
                _tileState = TileState.Exclamation;
                moveable = true;
                tileValue = Int32.Parse(data);
                _harvestable = false;
                _leftCornerText = Utils.CreateTextWorld($"{tileValue}", transform.position + new Vector3(0.3f, -0.3f), 24, transform, Color.white);
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
                _decreaseValue = false;
                moveable = true;
                _textToShow = data;
            }else if (data.Contains("P")) {
                moveable = true;
                _tileState = TileState.Pushing;
                _tileType = TileType.PushingTile;
                _textToShow = $"{data}";
                _decreaseValue = false;
                _harvestable = false;
            }

            ChangeTileColor();
        }

        public bool Harvest() {
            if (_harvestable) {
                if (tileValue == 0) {
                    //Good
                    GameManager.Instance.currentLevelGoal -= 1;
                    _tileState = TileState.GoodHarvested;
                    if (_tileImg) {
                        Destroy(_tileImg.gameObject);
                    }
                } else {
                    //Bad
                    _tileState = TileState.BadHarvested;
                    if (_tileImg) {
                        _tileImg.sprite = Resources.Load<Sprite>("icon_cross");
                    }
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
                    if (_tileType is TileType.ClassicTile or TileType.Moveable or TileType.PushingTile) {
                        _tileState = TileState.Normal;
                    }else if (_tileType == TileType.ExclamationTile) {
                        _tileState = TileState.Exclamation;
                    }
                }
                ChangeTileColor();
                return;
            }

            if (_canBreak) {
                moveable = false;
                _tileType = TileType.NotMoveable;
                _tileState = TileState.Invisible;
                TextUpdate("");
                ChangeTileColor();
                return;
            }


            tileValue--;

            if (tileValue == 0) {
                TextUpdate("");
                _tileImg = Utils.CreateSpriteWorld(Resources.Load<Sprite>("icon_outline_checkmark"), transform.position + new Vector3(0, 0, -1), new Vector2(0.5f,0.5f), this.transform);
            }

            //Refresh text
            // if (_textToShow.Contains("X") || _textToShow == "" || _textToShow.Length == 0) {
            //     
            //     return;
            // }

            if (tileValue < 0) {
                if (_tileType == TileType.ExclamationTile) {
                    _leftCornerText.text = "";
                    _tileType = TileType.ClassicTile;
                    _tileState = TileState.BadHarvested;
                    _tileImg.sprite = Resources.Load<Sprite>("icon_cross");
                }
                
                // if (_tileImg) {
                //     Destroy(_tileImg.gameObject);
                // }

                TextUpdate("");
                Harvest();
                ChangeTileColor();
            } else if(tileValue != 0){
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
                    if (_tileImg) {
                        Destroy(_tileImg.gameObject);
                    }

                    tileValue = _defaultTileValue + 1;
                    ChangeTileColor();
                    break;
                case TileType.PushingTile:
                    var pushDir = new Vector2Int();
                    if (_textToShow.Contains("U") && _textToShow.Contains("R")) {
                        // Move Up Right
                        pushDir = new Vector2Int(1,1);
                    }else if (_textToShow.Contains("U") && _textToShow.Contains("L")) {
                        // Move Up Left
                        pushDir = new Vector2Int(-1,1);
                    } else if (_textToShow.Contains("D") && _textToShow.Contains("R")) {
                        // Move Down Right
                        pushDir = new Vector2Int(1,-1);
                    } else if (_textToShow.Contains("D") && _textToShow.Contains("L")) {
                        // Move Down Left
                        pushDir = new Vector2Int(-1,-1);
                    } else if (_textToShow.Contains("U")) {
                        // Move Up
                        pushDir = new Vector2Int(0,1);
                    } else if (_textToShow.Contains("D")) {
                        // Move Down
                        pushDir = new Vector2Int(0,-1);
                    } else if (_textToShow.Contains("L")) {
                        // Move L
                        pushDir = new Vector2Int(-1,0);
                    } else if (_textToShow.Contains("R")) {
                        // Move R
                        pushDir = new Vector2Int(1,0);
                    } else {
                        return;
                    }
                    PushPlayer?.Invoke(pushDir);
                    break;
            }
        }

        public void OnTileStepAfter() {
            if (_breakable) {
                _canBreak = true;
            }
            
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
                    if (tile._tileState is TileState.Normal or TileState.Exclamation or TileState.Pushing) {
                        tile._tileState = TileState.Freeze;
                        tile.ChangeTileColor();
                    }
                }
            } else if (_tileType == TileType.FreezeTileVertical) {
                // Using GetColumn method to get all tiles in column
                GridManager.Instance.GetAllInColumn(gridPosition.x, out var allTiles);
                foreach (var tile in allTiles) {
                    tile._isFreeze = true;
                    if (tile._tileState is TileState.Normal or TileState.Exclamation or TileState.Pushing) {
                        tile._tileState = TileState.Freeze;
                        tile.ChangeTileColor();
                    }
                }
            }
            
            
        }
    }
}