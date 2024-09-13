using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.LevelEditor {
    public class TileEditor : MonoBehaviour {

        [SerializeField]
        private Color _baseColor, _offsetColor;

        [SerializeField]
        private SpriteRenderer _renderer;

        [SerializeField]
        private GameObject _highlight;

        public Vector2Int gridPosition;

        private TextMesh _textMesh;

        public int _harvestValue = -1;
        
        public string _editorTextValue = "";
        
        public void Init(bool isOffset) {
            _renderer.color = isOffset ? _offsetColor : _baseColor;
            _textMesh = Utils.CreateTextWorld($"{_editorTextValue}", transform.position, 32, transform, Color.white);
            _harvestValue = -1;
            _editorTextValue = "";
        }

        void OnMouseEnter() {
            _highlight.SetActive(true);
        }

        void OnMouseExit() {
            _highlight.SetActive(false);
        }

        private void OnMouseDown() {
           GridManagerEditor.Instance.SaveValueToTile(out string value);
           TileUpdate(value);
        }

        
        //Updating editor while edit a level
        public void TileUpdate(string text) {

            if (text.Contains("N")) {
                _harvestValue += 1;
                _editorTextValue = text + _harvestValue;
            }else if (text.Contains("!")) {
                _harvestValue += 1;
                _editorTextValue = text + _harvestValue;
            } else if (text.Contains("SP")) {
                GridManagerEditor.Instance.levelPlayerStartingPosition = gridPosition;
                return;
            }else {
                _harvestValue = -1;
                _editorTextValue = text;
            }
            _textMesh.text = _editorTextValue;
        }
        
        //Update tile on loading level in editor
        public void TileUpdate() {

            if (_editorTextValue.Contains("N")) {
                var data = _editorTextValue.Replace("N", string.Empty);
                _harvestValue = Int32.Parse(data);
                _editorTextValue = "N" + _harvestValue;
            }else if (_editorTextValue.Contains("!")) {
                var data = _editorTextValue.Replace("!", string.Empty);
                _harvestValue = Int32.Parse(data);
                _editorTextValue = "!" + _harvestValue;
            }else {
                _harvestValue = -1;
            }
            _textMesh.text = _editorTextValue;
        }
    }
}