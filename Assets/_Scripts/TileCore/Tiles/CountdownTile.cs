using System;
using System.Collections;
using _Scripts.Managers;
using _Scripts.TileCore.BaseClasses;
using UnityEngine;

namespace _Scripts.TileCore.Tiles {

    public sealed class CountdownTile : TileBase {
        public int countdownValue;

        private TileTextHandler _tileTextHandler;
        
        private bool _isCollected;

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

            if (countdownValue == 0) {
        

               
            }

            _tileTextHandler.middleText.text = countdownValue.ToString();
        }

        public override void OnPlayerStep() {
            base.OnPlayerStep();
            
        }
    }
}