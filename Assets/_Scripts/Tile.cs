using System;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts {
    public class Tile : MonoBehaviour {
        public static event UnityAction<Vector2Int> PlayerMove = delegate { };
        public static event UnityAction<Vector2Int> PushPlayer = delegate { };
        public static event UnityAction SpawnGhost = delegate { };
        public static event UnityAction<Vector2Int> RemoveGhost = delegate { };

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
        public bool _harvestable = false;
        public bool _decreaseValue = true;
        public bool _isFreeze = false;
        public TileType _tileType;
        private bool _isOffset;
        public TileState _tileState;
        private bool _canBreak = false;
        private bool _breakable = false;
        private SpriteRenderer _tileImg;
        public string _pushDirection;

        private void OnEnable() {
            PlayerBehaviour.DecreaseTileValueEvent += DecreaseValue;
        }

        private void OnDisable() {
            PlayerBehaviour.DecreaseTileValueEvent -= DecreaseValue;
        }

        public void Init(bool isOffset, string gridLevelData) {
            _isOffset = isOffset;
            ChangeTileSprite();

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
            if (_tileState == TileState.BadHarvested || _tileState == TileState.GoodHarvested) {
                _textToShow = "";
            } else {
                _textToShow = text;
            }

            switch (_tileType) {
                case TileType.ClassicTile:
                    _middleText.text = $"{_textToShow}";
                    break;
                case TileType.ExclamationTile:
                    //Have to remove ! if tileValue is 0
                    if (tileValue == 0) {
                        _middleText.text = $"{_textToShow}";
                    } else {
                        _middleText.text = $"!{_textToShow}";
                    }
                    break;
                case TileType.PushingTile:
                    _middleText.text = "";
                    break;
            }
        }

        private void ChangeTileSprite() {
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
                    _tileImg = Utils.CreateSpriteWorld(Resources.Load<Sprite>("button_round_flat"), transform.position + new Vector3(0, 0, -0.99f), new Vector2(0.5f,0.5f), this.transform);
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
                    _tileImg = Utils.CreateSpriteWorld(Resources.Load<Sprite>("FreezeHorizontal"), transform.position + new Vector3(0, 0, -0.99f), new Vector2(0.5f, 0.5f), this.transform);
                } else if (data.Contains("V")) {
                    _tileType = TileType.FreezeTileVertical;
                    _tileImg = Utils.CreateSpriteWorld(Resources.Load<Sprite>("FreezeVertical"), transform.position + new Vector3(0, 0, -0.99f), new Vector2(0.5f, 0.5f), this.transform);
                }
                _decreaseValue = false;
                moveable = true;
                _textToShow = "";
            }else if (data.Contains("P")) {
                moveable = true;
                _tileState = TileState.Pushing;
                _tileType = TileType.PushingTile;
                _textToShow = "";
                _pushDirection = data;
                _decreaseValue = false;
                _harvestable = false;
                
                if (data.Contains("U") && data.Contains("R")) {
                    // Move Up Right
                    _tileImg = Utils.CreateSpriteWorld(Resources.Load<Sprite>("ArrowsUpRight"), transform.position + new Vector3(0, 0, -0.99f), new Vector2(0.5f, 0.5f), this.transform);
                }else if (data.Contains("U") && data.Contains("L")) {
                    // Move Up Left
                    _tileImg = Utils.CreateSpriteWorld(Resources.Load<Sprite>("ArrowsUpLeft"), transform.position + new Vector3(0, 0, -0.99f), new Vector2(0.5f, 0.5f), this.transform);
                } else if (data.Contains("D") && data.Contains("R")) {
                    // Move Down Right
                    _tileImg = Utils.CreateSpriteWorld(Resources.Load<Sprite>("ArrowsDownRight"), transform.position + new Vector3(0, 0, -0.99f), new Vector2(0.5f, 0.5f), this.transform);
                } else if (data.Contains("D") && data.Contains("L")) {
                    // Move Down Left
                    _tileImg = Utils.CreateSpriteWorld(Resources.Load<Sprite>("ArrowsDownLeft"), transform.position + new Vector3(0, 0, -0.99f), new Vector2(0.5f, 0.5f), this.transform);
                } else if (data.Contains("U")) {
                    // Move Up
                    _tileImg = Utils.CreateSpriteWorld(Resources.Load<Sprite>("ArrowsUp"), transform.position + new Vector3(0, 0, -0.99f), new Vector2(0.5f, 0.5f), this.transform);
                } else if (data.Contains("D")) {
                    // Move Down
                    _tileImg = Utils.CreateSpriteWorld(Resources.Load<Sprite>("ArrowsDown"), transform.position + new Vector3(0, 0, -0.99f), new Vector2(0.5f, 0.5f), this.transform);
                } else if (data.Contains("L")) {
                    // Move L
                    _tileImg = Utils.CreateSpriteWorld(Resources.Load<Sprite>("ArrowsLeft"), transform.position + new Vector3(0, 0, -0.99f), new Vector2(0.5f, 0.5f), this.transform);
                } else if (data.Contains("R")) {
                    // Move R
                    _tileImg = Utils.CreateSpriteWorld(Resources.Load<Sprite>("ArrowsRight"), transform.position + new Vector3(0, 0, -0.99f), new Vector2(0.5f, 0.5f), this.transform);
                }
            }else if(data.Contains("S")) {
                moveable = true;
                _tileState = TileState.Normal;
                _tileType = TileType.SplitTile;
                _textToShow = "";
                _tileImg = Utils.CreateSpriteWorld(Resources.Load<Sprite>("Split"), transform.position + new Vector3(0, 0, -0.99f), new Vector2(0.5f, 0.5f), this.transform);
                _decreaseValue = false;
                _harvestable = false;
            }

            ChangeTileSprite();
        }

        public void Harvest() {
            if (_harvestable) {
                if (tileValue == 0) {
                    //Good
                    GameManager.Instance.currentLevelGoal -= 1;
                    _tileState = TileState.GoodHarvested;
                    if (_tileImg) {
                        Destroy(_tileImg.gameObject);
                    }
                    Debug.Log("Harvested good");

                } else {
                    //Bad
                    _tileState = TileState.BadHarvested;
                    if (_tileImg) {
                        Destroy(_tileImg.gameObject);
                    }
                    tileValue = -1;
                    _tileImg = Utils.CreateSpriteWorld(Resources.Load<Sprite>("icon_cross"), transform.position + new Vector3(0, 0, -0.99f), new Vector2(1f, 1f), this.transform);
                    Debug.Log("Harvested bad");

                }
                ChangeTileSprite();
                TextUpdate("");
                
                _harvestable = false;

                RemoveGhost?.Invoke(gridPosition);
            }
        }

        private void DecreaseValue() {
            
            if (_isFreeze) {
                _isFreeze = false;
                if(_tileState == TileState.Freeze) {
                    if (_tileType is TileType.ClassicTile or TileType.Moveable or TileType.PushingTile) {
                        _tileState = TileState.Normal;
                    }else if (_tileType == TileType.ExclamationTile) {
                        _tileState = TileState.Exclamation;
                    }else if (_tileType == TileType.SplitTile) {
                        _tileState = TileState.Normal;
                    }
                }
                ChangeTileSprite();
                return;
            }
            
            if (!_decreaseValue) {
                return;
            }

            if (_canBreak) {
                moveable = false;
                _tileType = TileType.NotMoveable;
                _tileState = TileState.Invisible;
                TextUpdate("");
                ChangeTileSprite();
                return;
            }

            tileValue--;

            if (tileValue == 0) {
                TextUpdate("");
                _tileImg = Utils.CreateSpriteWorld(Resources.Load<Sprite>("button_round_flat"), transform.position + new Vector3(0, 0, -0.99f), new Vector2(0.5f,0.5f), this.transform);
            }

            if (tileValue < 0) {
                if (_tileType == TileType.ExclamationTile) {
                    _leftCornerText.text = "";
                    _tileType = TileType.ClassicTile;
                    _tileState = TileState.BadHarvested;
                    
                    if (_tileImg) {
                        Destroy(_tileImg.gameObject);
                    }
                    _tileImg = Utils.CreateSpriteWorld(Resources.Load<Sprite>("icon_cross"), transform.position + new Vector3(0, 0, -0.99f), new Vector2(1f, 1f), this.transform);
                }

                TextUpdate("");
                Harvest();
                ChangeTileSprite();
            } else if(tileValue != 0){
                TextUpdate(tileValue.ToString());
            }
        }

        public bool OnTileStep() {
            switch (_tileType) {
                case TileType.ClassicTile:
                    Harvest();
                    return true;
                case TileType.ExclamationTile:
                    _harvestable = true;
                    _tileState = TileState.Normal;
                    _tileType = TileType.ClassicTile;
                    _leftCornerText.text = "";
                    
                    //Delete a image in case that ReadyToHarvest icon is shown
                    if (_tileImg) {
                        Destroy(_tileImg.gameObject);
                    }

                    tileValue = _defaultTileValue + 1;
                    ChangeTileSprite();
                    RemoveGhost?.Invoke(gridPosition);
                    return true;
                case TileType.PushingTile:
                    if (_tileState == TileState.Freeze) return true;
                    var pushDir = new Vector2Int();
                    if (_pushDirection.Contains("U") && _pushDirection.Contains("R")) {
                        // Move Up Right
                        pushDir = new Vector2Int(1,1);
                    }else if (_pushDirection.Contains("U") && _pushDirection.Contains("L")) {
                        // Move Up Left
                        pushDir = new Vector2Int(-1,1);
                    } else if (_pushDirection.Contains("D") && _pushDirection.Contains("R")) {
                        // Move Down Right
                        pushDir = new Vector2Int(1,-1);
                    } else if (_pushDirection.Contains("D") && _pushDirection.Contains("L")) {
                        // Move Down Left
                        pushDir = new Vector2Int(-1,-1);
                    } else if (_pushDirection.Contains("U")) {
                        // Move Up
                        pushDir = new Vector2Int(0,1);
                    } else if (_pushDirection.Contains("D")) {
                        // Move Down
                        pushDir = new Vector2Int(0,-1);
                    } else if (_pushDirection.Contains("L")) {
                        // Move L
                        pushDir = new Vector2Int(-1,0);
                    } else if (_pushDirection.Contains("R")) {
                        // Move R
                        pushDir = new Vector2Int(1,0);
                    } else {
                        return false;
                    }
                    PushPlayer?.Invoke(pushDir);
                    return true;
                case TileType.SplitTile:
                    if (_tileState == TileState.Freeze) return true;
                    // Spawn ghost player
                    SpawnGhost?.Invoke();
                    return true;
            }
            
            return false;
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
                        tile.ChangeTileSprite();
                    }
                }
            } else if (_tileType == TileType.FreezeTileVertical) {
                // Using GetColumn method to get all tiles in column
                GridManager.Instance.GetAllInColumn(gridPosition.x, out var allTiles);
                foreach (var tile in allTiles) {
                    tile._isFreeze = true;
                    if (tile._tileState is TileState.Normal or TileState.Exclamation or TileState.Pushing) {
                        tile._tileState = TileState.Freeze;
                        tile.ChangeTileSprite();
                    }
                }
            }
            
            
        }
    }
}