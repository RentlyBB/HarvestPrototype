using System;
using System.Collections;
using _Scripts.Managers;
using _Scripts.TileCore.BaseClasses;
using UnityEngine;

namespace _Scripts.TileCore.Tiles {

    public sealed class CountdownTile : TileBase {
        
        public int countdownValue;

        private TileTextHandler _tileTextHandler;
        
        protected override void Awake() {
            base.Awake();
            TryGetComponent(out _tileTextHandler);
        }

        private void OnEnable() {
            GameplayManager.CountdownDecreasing += DecreaseCountdownValue;
        }

        private void OnDisable() {
            GameplayManager.CountdownDecreasing -= DecreaseCountdownValue;
        }

        private void Start() {
            _tileTextHandler.middleText = Utils.CreateTextWorld(countdownValue.ToString(), new Vector3(transform.position.x, transform.position.y + 0.05f, -1), 40, transform, Color.green);
        }

        private void DecreaseCountdownValue() {
            countdownValue--;

            // TODO: Check if the state of tile is CountingDown            
            if (countdownValue == 0) {
               // TODO: Change state to ReadyToCollect and visual to CollectTile and disable textMeshPro
            }
            
            // Else change state to BadCollect and unsubscribe the CountdownDecreasingEvent

            _tileTextHandler.middleText.text = countdownValue.ToString();
        }

        public override void OnPlayerStep() {
            base.OnPlayerStep();
            //TODO: Check if the state of tile is ReadyToCollect
            // if True – Change state to Collected
            // if False – Change state to BadCollect and change sprite
            // also disable textMeshPro
        }
    }
}