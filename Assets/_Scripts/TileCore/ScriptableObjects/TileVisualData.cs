using System;
using System.Collections.Generic;
using _Scripts.TileCore.Composites;
using _Scripts.TileCore.Enums;
using UnityEngine;

namespace _Scripts.TileCore.ScriptableObjects {
    
    [CreateAssetMenu(fileName = "NewTileVisualData", menuName = "Tiles/TileVisualData")]
    public class TileVisualData : ScriptableObject {
        [Serializable]
        public class StateSpritePair {
            public TileCompositeState compositeState;
            public Sprite sprite;
        }

        public List<StateSpritePair> stateSpritesList = new List<StateSpritePair>();

        private Dictionary<TileCompositeState, Sprite> _stateSpritesDictionary;

        // Convert the list to a dictionary at runtime
        private void OnEnable() {
            _stateSpritesDictionary = new Dictionary<TileCompositeState, Sprite>();
            foreach (var pair in stateSpritesList) {
                if (!_stateSpritesDictionary.ContainsKey(pair.compositeState)) {
                    _stateSpritesDictionary[pair.compositeState] = pair.sprite;
                } else {
                    Debug.LogWarning($"Duplicate state entry for {pair.compositeState.mainState} - {pair.compositeState.subState} in {name}");
                }
            }
        }
        
        public Sprite GetSprite(TileMainVisualStates mainState, TileSubVisualStates subState) {

            var compositeState = new TileCompositeState(mainState, subState);
            if (_stateSpritesDictionary.TryGetValue(compositeState, out Sprite sprite)) {
                return sprite;
            }
            Debug.LogWarning($"Sprite not found for state {mainState} - {subState}");
            return null;
        }
    }
}