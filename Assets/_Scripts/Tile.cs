using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts {
    public class Tile : MonoBehaviour {

        public static event UnityAction<Vector2Int> PlayerMove = delegate { };

        [SerializeField] private Color _baseColor, _offsetColor, _harvestedColor, _badHarvestColor;
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private GameObject _highlight;

        public Vector2Int gridPosition;
        public int tileValue;
        public bool moveable;
        

        private bool _mouseOnTile = false;
        private TextMesh _textMesh;
        private string _textToShow;
        public bool _harvestable = false;

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

        private void DecreaseValue() {
            
            tileValue--;

            //Refresh text
            if (_textToShow.Contains("X") || _textToShow == "" || _textToShow.Length == 0) {
                return;
            }
            
            if (tileValue < 0) {
                TextUpdate("");
                Harvest();
            } else {
                TextUpdate(tileValue.ToString());
            }
        }

        private void TextUpdate(string text) {
            _textToShow = text;
            _textMesh.text = $"{_textToShow}";
        }

        private void EvaluateLevelData(string data) {
            if (data.Contains("X")) {
                moveable = false;
                _textToShow = "X";
            }else if (data.Contains("M")) {
                moveable = true;
                _textToShow = "";
            }else if (data.Contains("N")) {

              data = data.Replace("N", string.Empty);
              moveable = true;
              _harvestable = true;
              tileValue = Int32.Parse(data); 
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

    }
}