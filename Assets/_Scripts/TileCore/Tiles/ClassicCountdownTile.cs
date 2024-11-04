using System;
using System.Collections;
using _Scripts.TileCore.BaseClasses;
using _Scripts.TileCore.Interfaces;
using EditorScripts;
using UnityEngine;

namespace _Scripts.TileCore.Tiles {
    public class ClassicCountdownTile : CountdownTileBase, IInteractableTile {

        private TextMesh _middleText;

        private void Start() {
            _middleText = Utils.CreateTextWorld(countdownValue.ToString(), transform.position, 32, transform);
        }

        public void OnPlayerStep() {
            CheckCountdown();
        }

        protected override void CheckCountdown() {
            if (countdownValue == 0) {
                //TODO: Add 1 to score and switch tile from countdown to goodTile
            } else {
                //TODO: switch tile from countdown to goodTile
            }
        }

        [InvokeButton]
        private void DecreaseCountdownValue() {
            countdownValue--;
        }
    }
}